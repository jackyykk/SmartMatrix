using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController<AuthController>
    {
        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
        {
        }

        [HttpGet("google/login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleCallback", "Auth", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback()
        {            
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return Unauthorized(); // Handle failure

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
    }    
}