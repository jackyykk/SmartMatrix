using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/google")]
    public class GoogleController : BaseController<GoogleController>
    {
        const string LOGIN_PROVIDER_NAME = SysLogin.LoginProviderOptions.Google;

        public GoogleController(ILogger<GoogleController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        private string GenerateState()
        {
            var state = new
            {
                Random = new Random().Next(100000, 999999).ToString(),
                Timestamp = DateTime.UtcNow.Ticks
            };
            
            return JsonSerializer.Serialize(state);
        }

        private bool ValidateState(string state)
        {
            // Implement your state validation logic here
            // For example, you can deserialize the state and check its contents
            try
            {
                var stateObj = JsonSerializer.Deserialize<dynamic>(state);
                // Add your validation logic here
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Login by Google, redirect to Google login page
        [HttpGet("login")]
        public IActionResult Login()
        {            
            var state = GenerateState();            
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Google", new { state }, Request.Scheme);
            var properties = new AuthenticationProperties
            {                
                RedirectUri = redirectUrl,                
                Items = { { "state", state } },
                AllowRefresh = true
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        
        // Callback from Google after login
        // Don't need to setup the route here because it's already defined in the Login method
        [HttpGet()]
        public async Task<IActionResult> GoogleCallback()
        {            
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)                
                return Ok(Result<SysUser_PerformLogin_Response>.Fail("Unauthorized"));

            // Validate the state parameter
            if (!result.Properties.Items.TryGetValue("state", out var state) || state == null || !ValidateState(state))
                return Ok(Result<SysUser_PerformLogin_Response>.Fail("Invalid state parameter"));

            // Extract user information
            var claims = result.Principal?.Claims;
            if (claims == null)
                return Ok(Result<SysUser_PerformLogin_Response>.Fail("Invalid claims"));
                        
            var googleUserProfile = Auth_Google_Get_UserProfile(claims);
            SysUser_OutputPayload userPayload;
            SysToken_OutputPayload tokenPayload;

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
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, SysUser_PerformLogin_Response.StatusTexts.Unknown_Error));
            
            if (getUserResult.Data.User == null)
            {                
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
                if (!getUserProfileResult.Succeeded || getUserProfileResult.Data == null)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, SysUser_PerformLogin_Response.StatusTexts.Unknown_Error));

                var userProfile = getUserProfileResult.Data.User;

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
                foreach(var role in userProfile.Roles)
                {
                    var newRole = _mapper.Map<SysRole_InputPayload>(role);
                    newUser.Roles.Add(newRole);
                }

                var user = _mapper.Map<SysUser>(newUser);
                var token = Auth_Generate_SysToken(LOGIN_PROVIDER_NAME, googleUserProfile.Email, user);

                if (token == null)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Token_Generation_Failed, SysUser_PerformLogin_Response.StatusTexts.Unknown_Error));
                }

                var newLogin = new SysLogin_InputPayload
                {
                    PartitionKey = SysLogin_OutputPayload.PartitionKeyOptions.SmartMatrix,
                    LoginProvider = LOGIN_PROVIDER_NAME,
                    LoginType = SysLogin_OutputPayload.LoginTypeOptions.Web,
                    LoginName = googleUserProfile.Email,
                    RefreshToken = token.RefreshToken,
                    RefreshTokenExpires = token.RefreshToken_Expires,
                    Status = SysLogin_OutputPayload.StatusOptions.Active,                    
                };

                newUser.Logins.Add(newLogin);
                
                var insertUserResult = await _mediator.Send(new SysUser_InsertUser_Command
                {
                    Request = new SysUser_InsertUser_Request
                    {
                        User = newUser
                    }
                });
                
                if (!insertUserResult.Succeeded || insertUserResult.Data == null || insertUserResult.Data.User == null)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.User_Insert_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));

                // Output the user and token
                userPayload = insertUserResult.Data.User;
                tokenPayload = _mapper.Map<SysToken_OutputPayload>(token);
            }
            else
            {
                // Check if the login is already registered
                userPayload = getUserResult.Data.User;
                var loginName = googleUserProfile.Email;
                var loginPayload = userPayload.Logins.FirstOrDefault(x => x.LoginName == loginName);

                // Suppose user should have the login
                if (loginPayload == null)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Login_NotFound, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                }                 

                // Step 2: Generate token
                var user = _mapper.Map<SysUser>(userPayload);
                var token = Auth_Generate_SysToken(LOGIN_PROVIDER_NAME, loginName, user);

                if (token == null)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Token_Generation_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                }

                loginPayload.RefreshToken = token.RefreshToken;
                loginPayload.RefreshTokenExpires = token.RefreshToken_Expires;

                // Step 3: Update refresh token
                var updateRefreshTokenResult = await _mediator.Send(new SysLogin_UpdateRefreshToken_Command
                {
                    Request = new SysLogin_UpdateRefreshToken_Request
                    {
                        LoginId = loginPayload.Id,
                        RefreshToken = token.RefreshToken,
                        RefreshTokenExpires = token.RefreshToken_Expires
                    }
                });

                if (!updateRefreshTokenResult.Succeeded)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.RefreshToken_Update_Failed, SysUser_PerformLogin_Response.StatusTexts.Login_Failed));
                }

                tokenPayload = _mapper.Map<SysToken_OutputPayload>(token);
            }                        

            var response = new SysUser_PerformLogin_Response
            {
                User = userPayload,
                Token = tokenPayload
            };

            return Ok(Result<SysUser_PerformLogin_Response>.Success(response));
        }                
    }    
}