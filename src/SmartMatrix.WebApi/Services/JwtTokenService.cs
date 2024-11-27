using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using static SmartMatrix.Domain.Constants.CommonConstants.Identities;

namespace SmartMatrix.WebApi.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly ILogger<JwtTokenService> _logger;
        private readonly IConfiguration _configuration;

        public JwtTokenService(ILogger<JwtTokenService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        #region  Encode/Decode

        protected string EncodeJwt(SysTokenContent content)
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

            // Add roles as claims
            foreach (var role in content.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(content.Secret.Jwt_Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);            

            var token = new JwtSecurityToken(
                content.Secret.Jwt_Issuer,
                content.Secret.Jwt_Audience,
                claims,
                expires: content.Secret.AccessToken_Expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        protected SysTokenContent DecodeJwt(SysSecret secret, string token)
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
                ValidIssuer = secret.Jwt_Issuer,
                ValidAudience = secret.Jwt_Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret.Jwt_Key))
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

        protected static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        #endregion

        protected SysSecret GetTokenSecret()
        {
            SysSecret secret;
            string accessToken_Format = _configuration["Authentication:AccessToken:Format"] ?? string.Empty;
            double accessToken_LifeInMinutes = _configuration.GetValue<double>("Authentication:AccessToken:LifeInMinutes");            
            string refreshToken_Format = _configuration["Authentication:RefreshToken:Format"] ?? string.Empty;
            double refreshToken_LifeInMinutes = _configuration.GetValue<double>("Authentication:RefreshToken:LifeInMinutes");

            DateTime accessToken_Expires = DateTime.UtcNow.AddMinutes(accessToken_LifeInMinutes);
            DateTime refreshToken_Expires = DateTime.UtcNow.AddMinutes(refreshToken_LifeInMinutes);

            if (!string.Equals(accessToken_Format, "Jwt", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("Invalid access token format");
            }
            if (!string.Equals(refreshToken_Format, "Standard", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("Invalid refresh token format");
            }
            
            string jwtKey = _configuration["Authentication:Jwt:Key"] ?? string.Empty;
            string jwtIssuer = _configuration["Authentication:Jwt:Issuer"] ?? string.Empty;
            string jwtAudience = _configuration["Authentication:Jwt:Audience"] ?? string.Empty;    

            // Create JWT token
            secret = new SysSecret
            {                
                Jwt_Key = jwtKey ?? string.Empty,
                Jwt_Issuer = jwtIssuer ?? string.Empty,
                Jwt_Audience = jwtAudience ?? string.Empty,
                AccessToken_Format = accessToken_Format,
                AccessToken_LifeInMinutes = accessToken_LifeInMinutes,
                AccessToken_Expires = accessToken_Expires,
                RefreshToken_Format = refreshToken_Format,
                RefreshToken_LifeInMinutes = refreshToken_LifeInMinutes,
                RefreshToken_Expires = refreshToken_Expires
            };
                                                        
            return secret;
        }

        public SysToken GenerateToken(SysTokenContent content)
        {
            SysToken token = new SysToken();
            
            if (content.Secret == null || string.IsNullOrEmpty(content.Secret.AccessToken_Format))
            {
                throw new System.Exception("Invalid secret");
            }

            if (!string.Equals(content.Secret.AccessToken_Format, "Jwt", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("Invalid access token format");
            }

            if (!string.Equals(content.Secret.RefreshToken_Format, "Standard", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("Invalid refresh token format");
            }

            // Only support Jwt for now
            var accessToken = EncodeJwt(content);

            // Only support Standard for now
            var refreshToken = GenerateRefreshToken();
            
            token.AccessToken = accessToken;
            token.AccessToken_LifeInMinutes = content.Secret.AccessToken_LifeInMinutes;
            token.AccessToken_Expires = content.Secret.AccessToken_Expires;            
            token.RefreshToken = refreshToken;
            token.RefreshToken_LifeInMinutes = content.Secret.RefreshToken_LifeInMinutes;
            token.RefreshToken_Expires = content.Secret.RefreshToken_Expires;

            return token;
        }

        public SysToken GenerateToken(string provider, string loginName, SysUser user)
        {            
            SysSecret secret = GetTokenSecret();

            SysTokenContent content = new SysTokenContent
            {
                Secret = secret,
                LoginProviderName = provider ?? string.Empty,
                LoginNameIdentifier = loginName ?? string.Empty,                
                UserNameIdentifier = user?.UserName ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                Name = user?.DisplayName ?? string.Empty,
                GivenName = user?.GivenName ?? string.Empty,
                Surname = user?.Surname ?? string.Empty,
                Roles = user?.UserRoles?.Select(x => x.Role?.RoleCode).ToList()
            };
                    
            return GenerateToken(content);
        }
    }
}