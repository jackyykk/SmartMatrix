using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Externals;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/google")]
    public class GoogleController : BaseController<GoogleController>
    {        
        const string LOGIN_PROVIDER_NAME = SysLogin.LoginProviderOptions.Google;
        public string Default_UserProfile = SysUser_OutputPayload.TypeOptions.BuiltIn_Normal_User_Profile;

        protected readonly ITokenService _tokenService;

        public GoogleController(ILogger<GoogleController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper, ITokenService tokenService)
            : base(logger, configuration, mediator, mapper)
        {
            _tokenService = tokenService;
        }

        public class LoginState
        {
            public string? Random { get; set; }
            public string? OriginUrl { get; set; }
            public string? ReturnUrl { get; set; }
            public long Timestamp { get; set; }
        }

        private string GenerateLoginState(string originUrl, string returnUrl)
        {
            var state = new LoginState
            {
                Random = new Random().Next(100000, 999999).ToString(),
                OriginUrl = originUrl,
                ReturnUrl = returnUrl,
                Timestamp = DateTime.UtcNow.Ticks
            };

            return JsonSerializer.Serialize(state);
        }

        private LoginState? RetrieveLoginState(string json)
        {
            // Implement your state validation logic here
            // For example, you can deserialize the state and check its contents
            LoginState? state;

            try
            {
                var stateObj = JsonSerializer.Deserialize<LoginState>(json);                
                state = stateObj;
            }
            catch
            {
                state = null;
            }

            return state;
        }

        private GoogleUserProfile GetUserProfile(IEnumerable<Claim> claims)
        {
            var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var surname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            var pictureUrl = claims?.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

            GoogleUserProfile userProfile = new GoogleUserProfile
            {
                UserId = userId,
                Email = email,
                UserName = userName,
                GivenName = givenName,
                Surname = surname,
                PictureUrl = pictureUrl
            };

            return userProfile;
        }

        /// <summary>
        /// Login by Google, redirect to Google login page
        /// </summary>
        /// <returns>SysUser_PerformLogin_Response</returns>
        [HttpGet("login")]
        public IActionResult Login(string originUrl, string returnUrl)
        {
            try
            {
                var state = GenerateLoginState(originUrl, returnUrl);
                var redirectUrl = Url.Action(nameof(GoogleCallback), "Google", new { state }, Request.Scheme);
                var properties = new AuthenticationProperties
                {
                    RedirectUri = redirectUrl,
                    Items = { { "state", state } },
                    AllowRefresh = true
                };

                return Challenge(properties, GoogleDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex)));
            }
        }

        /// <summary>
        /// Callback from Google after login, Don't need to setup the route here because it's already defined in the Login method
        /// </summary>
        /// <returns>SysUser_PerformLogin_Response</returns>        
        [HttpGet()]
        public async Task<IActionResult> GoogleCallback()
        {
            // Output variables
            SysUser_OutputPayload outputUser;
            SysToken_OutputPayload outputToken;

            try
            {                
                var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

                if (!result.Succeeded)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail("Unauthorized"));

                // Validate the state parameter
                if (!result.Properties.Items.TryGetValue("state", out var state) || state == null)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail("Invalid state parameter"));

                var loginState = RetrieveLoginState(state);
                if (loginState == null)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail("Invalid state parameter"));
                
                // Extract user information
                var claims = result.Principal?.Claims;
                if (claims == null)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail("Invalid claims"));

                var googleUserProfile = GetUserProfile(claims);                

                // Check if the user is already registered
                var getUserResult = await _mediator.Send(new SysUser_GetFirstByLoginName_Query
                {
                    Request = new SysUser_GetFirstByLoginName_Request
                    {
                        PartitionKey = SysUser_OutputPayload.PartitionKeyOptions.SmartMatrix,    // Use SmartMatrix as the partition key
                        LoginName = googleUserProfile.Email
                    }
                });

                if (!getUserResult.Succeeded || getUserResult.Data == null)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));

                if (getUserResult.Data.User == null)
                {

                    if (Default_UserProfile != SysUser_OutputPayload.TypeOptions.BuiltIn_Normal_User_Profile)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Configuration_Error, SysUser_PerformLogin_Response.StatusTexts.Configuration_Error));
                    }

                    // Register the user as normal user if not exists
                    var getUserProfileResult = await _mediator.Send(new SysUser_GetFirstByType_Query
                    {
                        Request = new SysUser_GetFirstByType_Request
                        {
                            PartitionKey = SysUser_OutputPayload.PartitionKeyOptions.SmartMatrix,
                            Type = SysUser_OutputPayload.TypeOptions.BuiltIn_Normal_User_Profile   // Use BuiltIn_Normal_User_Profile as the template
                        }
                    });

                    // Check if the built-in normal user profile exists
                    if (!getUserProfileResult.Succeeded || getUserProfileResult.Data == null || getUserProfileResult.Data.User == null)
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.UserProfile_NotFound, SysUser_PerformLogin_Response.StatusTexts.Configuration_Error));

                    var existingUserProfile = getUserProfileResult.Data.User;

                    if (existingUserProfile.Status == SysUser_OutputPayload.StatusOptions.Disabled)
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.UserProfile_Disabled, SysUser_PerformLogin_Response.StatusTexts.Configuration_Error));

                    if (existingUserProfile.Status == SysUser_OutputPayload.StatusOptions.Deleted || existingUserProfile.IsDeleted)
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.UserProfile_Deleted, SysUser_PerformLogin_Response.StatusTexts.Configuration_Error));

                    // Get normal user profile and Register the user as normal user
                    var newUser = new SysUser_InputPayload
                    {
                        PartitionKey = SysUser_OutputPayload.PartitionKeyOptions.SmartMatrix,
                        Type = SysUser_OutputPayload.TypeOptions.Normal_User,
                        UserName = googleUserProfile.Email,
                        DisplayName = googleUserProfile.UserName,
                        GivenName = googleUserProfile.GivenName,
                        Surname = googleUserProfile.Surname,
                        Email = googleUserProfile.Email,
                        Status = SysUser_OutputPayload.StatusOptions.Active,
                    };

                    // Copy the roles of user profile to the new user
                    foreach (var role in existingUserProfile.Roles)
                    {
                        var newRole = _mapper.Map<SysRole_InputPayload>(role);
                        newUser.Roles.Add(newRole);
                    }

                    var user = _mapper.Map<SysUser>(newUser);
                    foreach(var userRole in user.UserRoles)
                    {
                        Console.WriteLine(userRole.SysUserId);
                        Console.WriteLine(userRole.SysRoleId);
                    }

                    var token = _tokenService.GenerateToken(LOGIN_PROVIDER_NAME, googleUserProfile.Email, user);

                    if (token == null)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Token_Generation_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                    }

                    // Add the login to the user
                    var newLogin = new SysLogin_InputPayload
                    {
                        PartitionKey = SysLogin_OutputPayload.PartitionKeyOptions.SmartMatrix,
                        LoginProvider = LOGIN_PROVIDER_NAME,
                        LoginType = SysLogin_OutputPayload.LoginTypeOptions.Web,
                        LoginName = googleUserProfile.Email,
                        RefreshToken = token.RefreshToken,
                        RefreshTokenExpires = token.RefreshToken_Expires,
                        OneTimeToken = token.OneTimeToken,
                        OneTimeTokenExpires = token.OneTimeToken_Expires,
                        Status = SysLogin_OutputPayload.StatusOptions.Active,
                        PictureUrl = googleUserProfile.PictureUrl
                    };

                    newUser.Logins.Add(newLogin);

                    var createUserResult = await _mediator.Send(new SysUser_CreateUser_Command
                    {
                        Request = new SysUser_CreateUser_Request
                        {
                            User = newUser
                        }
                    });

                    if (!createUserResult.Succeeded || createUserResult.Data == null || createUserResult.Data.User == null)
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.User_Insert_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));

                    // Output the user and token
                    outputUser = createUserResult.Data.User;
                    outputToken = _mapper.Map<SysToken_OutputPayload>(token);
                }
                else
                {
                    // Check if the login is already registered
                    var existingUser = getUserResult.Data.User;
                    var loginName = googleUserProfile.Email;
                    var existingLogin = existingUser.Logins.FirstOrDefault(x => x.LoginName == loginName);

                    // Suppose user should have the login
                    if (existingLogin == null)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Login_NotFound, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                    }

                    // Step 2: Generate token
                    var user = _mapper.Map<SysUser>(existingUser);
                    var token = _tokenService.GenerateToken(LOGIN_PROVIDER_NAME, loginName, user);

                    if (token == null)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Token_Generation_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                    }

                    existingLogin.RefreshToken = token.RefreshToken;
                    existingLogin.RefreshTokenExpires = token.RefreshToken_Expires;
                    existingLogin.OneTimeToken = token.OneTimeToken;
                    existingLogin.OneTimeTokenExpires = token.OneTimeToken_Expires;

                    // Step 3: Update refresh token
                    var updateTokensResult = await _mediator.Send(new SysLogin_UpdateTokens_Command
                    {
                        Request = new SysLogin_UpdateTokens_Request
                        {
                            LoginId = existingLogin.Id,
                            RefreshToken = token.RefreshToken,
                            RefreshTokenExpires = token.RefreshToken_Expires,
                            OneTimeToken = token.OneTimeToken,
                            OneTimeTokenExpires = token.OneTimeToken_Expires
                        }
                    });

                    if (!updateTokensResult.Succeeded)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.RefreshToken_Update_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                    }

                    // Output the user and token
                    outputUser = existingUser;
                    outputToken = _mapper.Map<SysToken_OutputPayload>(token);
                }

                var response = new SysUser_PerformLogin_Response
                {
                    LoginName = googleUserProfile.Email,
                    UserName = outputUser.UserName,
                    User = outputUser,
                    Token = outputToken
                };

                // Use redirect to return one time token instead of response
                //return Ok(Result<SysUser_PerformLogin_Response>.Success(response));

                string originUrl = loginState?.OriginUrl!;                                
                string returnUrl = loginState?.ReturnUrl!; 
                // Encode returnUrl and OneTimeToken
                string encodedReturnUrl = Uri.EscapeDataString(returnUrl);                
                string encodedOneTimeToken = Uri.EscapeDataString(response.Token.OneTimeToken);
                
                // Redirect to returnUrl
                return Redirect($"{originUrl}?ts=true&ott={encodedOneTimeToken}&returnUrl={encodedReturnUrl}");
            }
            catch (Exception ex)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex)));
            }
        }
    }
}