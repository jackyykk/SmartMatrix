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
        public static string EncodeJwt(SysTokenContent content)
        {            
            var claims = new List<Claim>
            {
                new Claim(SysClaimTypes.LoginProviderName, content.LoginProviderName),
                new Claim(SysClaimTypes.LoginNameIdentifier, content.LoginNameIdentifier),                
                new Claim(ClaimTypes.NameIdentifier, content.UserNameIdentifier),
                new Claim(ClaimTypes.Email, content.Email),
                new Claim(ClaimTypes.Name, content.Name),
                new Claim(ClaimTypes.GivenName, content.GivenName),
                new Claim(ClaimTypes.Surname, content.Surname),                                            
            };            

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(content.Secret.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);            

            var token = new JwtSecurityToken(
                content.Secret.Issuer,
                content.Secret.Audience,
                claims,
                expires: content.Secret.Expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static SysTokenContent DecodeJwt(SysSecret secret, string token)
        {
            SysTokenContent content = new SysTokenContent
            {
                Secret = secret.Copy()
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = secret.Issuer,
                ValidAudience = secret.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.Key))
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            content.LoginProviderName = principal.FindFirst(SysClaimTypes.LoginProviderName)?.Value;
            content.LoginNameIdentifier = principal.FindFirst(SysClaimTypes.LoginNameIdentifier)?.Value;            
            content.UserNameIdentifier = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            content.Email = principal.FindFirst(ClaimTypes.Email)?.Value;
            content.Name = principal.FindFirst(ClaimTypes.Name)?.Value;
            content.GivenName = principal.FindFirst(ClaimTypes.GivenName)?.Value;
            content.Surname = principal.FindFirst(ClaimTypes.Surname)?.Value;

            return content;
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