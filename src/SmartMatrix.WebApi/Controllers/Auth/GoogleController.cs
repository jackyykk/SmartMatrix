using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartMatrix.Domain.Core.Identities.Others;
using SmartMatrix.WebApi.Utils;

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth/google")]
    public class GoogleController : BaseController<GoogleController>
    {
        const string AUTH_PROVIDER_NAME = "Google";        

        public GoogleController(ILogger<GoogleController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
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
                return Unauthorized(); // Handle failure

            // Validate the state parameter
            if (!result.Properties.Items.TryGetValue("state", out var state) || state == null || !ValidateState(state))
                return BadRequest("Invalid state parameter");

            // Extract user information
            var claims = result.Principal?.Claims;
            var nameidentifier = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var surname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            var profilePicture = claims?.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

            // Save user information to database, email is primary key


            // Get Jwt Content
            string jwtKey = _configuration["Authentication:Jwt:Key"] ?? string.Empty;
            string jwtIssuer = _configuration["Authentication:Jwt:Issuer"] ?? string.Empty;
            string jwtAudience = _configuration["Authentication:Jwt:Audience"] ?? string.Empty;
            double jwtExpiresInMinutes = _configuration.GetValue<double>("Authentication:Jwt:ExpiresInMinutes");
            DateTime jwtExpires = DateTime.UtcNow.AddMinutes(jwtExpiresInMinutes);

            // Create JWT token
            JwtContent jwtContent = new JwtContent
            {
                Key = jwtKey,
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                Expires = jwtExpires,
                AuthProviderName = AUTH_PROVIDER_NAME,
                NameIdentifier = nameidentifier,
                Email = email,
                Name = name,
                GivenName = givenName,
                Surname = surname                
            };
            var authToken = TokenUtil.GenerateJwt(jwtContent);
            var refreshToken = TokenUtil.GenerateRefreshToken();
            double refreshToken_ExpiresInMinutes = _configuration.GetValue<double>("Authentication:RefreshToken:ExpiresInMinutes");
            DateTime refreshToken_Expires = DateTime.UtcNow.AddMinutes(refreshToken_ExpiresInMinutes);

            return Ok(new
            {                
                email,
                name,
                givenName,
                surname,
                profilePicture,
                authToken,
                refreshToken,
                refreshToken_Expires
            });
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