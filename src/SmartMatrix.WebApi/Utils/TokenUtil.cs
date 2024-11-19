using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmartMatrix.Domain.Core.Identities;
using static SmartMatrix.Domain.Constants.CommonConstants.Identities;

namespace SmartMatrix.WebApi.Utils
{
    public static class TokenUtil
    {
        public static string GenerateJwt(JwtContent content)
        {            
            var claims = new List<Claim>
            {
                new Claim(SysClaimTypes.LoginNameIdentifier, content.LoginNameIdentifier),
                new Claim(ClaimTypes.Sid, content.Sid),
                new Claim(ClaimTypes.NameIdentifier, content.UserNameIdentifier),
                new Claim(ClaimTypes.Email, content.Email),
                new Claim(ClaimTypes.Name, content.Name),
                new Claim(ClaimTypes.GivenName, content.GivenName),
                new Claim(ClaimTypes.Surname, content.Surname),                                            
            };            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(content.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);            

            var token = new JwtSecurityToken(
                content.Issuer,
                content.Audience,
                claims,
                expires: content.Expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}