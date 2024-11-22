using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Core.BaseClasses.Web;
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
            var token = Auth_Google_Generate_SysToken(LOGIN_PROVIDER_NAME, googleUserProfile);

            // Check if the user is already registered
            var getUserResult = await _mediator.Send(new SysUser_GetFirstByLoginName_Query
            {
                Request = new SysUser_GetFirstByLoginName_Request
                {
                    PartitionKey = SysLogin.PartitionKeyOptions.SmartMatrix,    // Use SmartMatrix as the partition key
                    LoginName = googleUserProfile.Email
                }
            });

            if (!getUserResult.Succeeded || getUserResult.Data == null)
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, SysUser_PerformLogin_Response.StatusTexts.Unknown_Error));

            if (getUserResult.Data.User == null)
            {
                var getUserProfileResult = await _mediator.Send(new SysUser_GetFirstByType_Query
                {
                    Request = new SysUser_GetFirstByType_Request
                    {
                        PartitionKey = SysUser.PartitionKeyOptions.SmartMatrix,
                        Type = SysUser.TypeOptions.BuiltIn_Normal_User_Profile
                    }
                });
                
                if (!getUserProfileResult.Succeeded || getUserProfileResult.Data == null)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, SysUser_PerformLogin_Response.StatusTexts.Unknown_Error));

                var userProfile = getUserProfileResult.Data.User;

                // Get normal user profile and Register the user as normal user
                var newUser = new SysUser
                {
                    PartitionKey = SysUser.PartitionKeyOptions.SmartMatrix,
                    Type = SysUser.TypeOptions.Normal_User,
                    UserName = googleUserProfile.Email,
                    DisplayName = googleUserProfile.UserName,
                    GivenName = googleUserProfile.GivenName,
                    Surname = googleUserProfile.Surname,
                    Email = googleUserProfile.Email,
                    Status = SysUser.StatusOptions.Active,                    
                };

                var newLogin = new SysLogin
                {
                    PartitionKey = SysLogin.PartitionKeyOptions.SmartMatrix,
                    LoginProvider = LOGIN_PROVIDER_NAME,
                    LoginType = SysLogin.LoginTypeOptions.Web,
                    LoginName = googleUserProfile.Email,
                    RefreshToken = token.RefreshToken,
                    RefreshTokenExpires = token.RefreshToken_Expires,
                    Status = SysLogin.StatusOptions.Active,                    
                };

                newUser.Logins.Add(newLogin);

                // Copy the roles of user profile to the new user
                foreach(var role in userProfile.Roles)
                {
                    newUser.Roles.Add(SysRole.Copy(role));
                }

                var insertUserResult = await _mediator.Send(new SysUser_InsertUser_Command
                {
                    Request = new SysUser_InsertUser_Request
                    {
                        User = newUser
                    }
                });
                
                if (!insertUserResult.Succeeded)
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail("User registration failed"));
            }
            else
            {
                // Check if the login is already registered
                
            }

            var tokenPayload = _mapper.Map<SysTokenPayload>(token);

            var response = new SysUser_PerformLogin_Response
            {
                Token = tokenPayload
            };

            return Ok(Result<SysUser_PerformLogin_Response>.Success(response));
        }                
    }    
}