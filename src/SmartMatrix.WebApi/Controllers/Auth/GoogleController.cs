using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.WebApi.Utils;

namespace SmartMatrix.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/google")]
    public class GoogleController : BaseController<GoogleController>
    {
        const string LOGIN_PROVIDER_NAME = "Google";

        public GoogleController(ILogger<GoogleController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        [HttpGet("login")]
        public IActionResult Login()
        {            
            var state = GenerateState();            
            var redirectUrl = Url.Action(nameof(GoogleResponse), "Google", new { state }, Request.Scheme);
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
        public async Task<IActionResult> GoogleResponse()
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

            var userProfile = Auth_Google_Get_UserProfile(claims);
            var token = Auth_Google_Generate_SysToken(LOGIN_PROVIDER_NAME, userProfile);

            // Check if the user is already registered

            var response = new SysUser_PerformLogin_Response
            {

                Token = token
            };  
            return Ok(Result<SysUser_PerformLogin_Response>.Success(response));
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
    }    
}