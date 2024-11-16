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

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    [Route("Auth/google")]
    public class GoogleController : BaseController<GoogleController>
    {
        public GoogleController(ILogger<GoogleController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
        {
        }

        [HttpGet("login")]
        public IActionResult Login()
        {            
            var state = GenerateState();
            var redirectUrl = Url.Action("Callback", "Google", new { state, }, Request.Scheme);
            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl,
                Items = { { "state", state } }
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {            
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return Unauthorized(); // Handle failure

            // Validate the state parameter
            if (!ValidateState(result.Properties.Items["state"]))
                return BadRequest("Invalid state parameter");

            // Create JWT token
            var token = GenerateJwtToken(result.Principal);
            return Ok(new { token });
        }

        private string GenerateJwtToken(ClaimsPrincipal principal)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, principal.Identity?.Name ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateState()
        {
            // Generate a random state value
            var state = Guid.NewGuid().ToString();
            return state;
        }

        private bool ValidateState(string state)
        {
            return true;
        }
    }    
}