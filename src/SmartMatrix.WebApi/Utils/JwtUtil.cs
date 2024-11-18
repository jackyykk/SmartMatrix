using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SmartMatrix.WebApi.Utils
{
    public static class JwtUtil
    {
        public static string GenerateJwt(string jwtKey, string jwtIssuer, string jwtAudience,  ClaimsPrincipal? principal, string providerName)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, principal?.Identity?.Name ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(30);

            var token = new JwtSecurityToken(
                "SmartMatrix",
                "SmartMatrix",
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}