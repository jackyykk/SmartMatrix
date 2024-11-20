using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartMatrix.Domain.Configurations;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.WebApi.Utils;

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        protected readonly IConfiguration _configuration;
        protected readonly IMediator _mediator;

        protected BaseController(ILogger<T> logger, IConfiguration configuration, IMediator mediator)
        {
            _logger = logger;
            _configuration = configuration;
            _mediator = mediator;
        }

        private TokenContent Auth_Generate_TokenContent(JwtContent jwtContent)
        {
            TokenContent token = new TokenContent();

            var authToken = TokenUtil.GenerateJwt(jwtContent);
            var refreshToken = TokenUtil.GenerateRefreshToken();
            double refreshToken_ExpiresInMinutes = _configuration.GetValue<double>("Authentication:RefreshToken:ExpiresInMinutes");
            DateTime refreshToken_Expires = DateTime.UtcNow.AddMinutes(refreshToken_ExpiresInMinutes);

            token.AuthToken = authToken;
            token.AuthExpires = jwtContent.Expires;
            token.RefreshToken = refreshToken;
            token.RefreshTokenExpires = refreshToken_Expires;

            return token;
        }

        protected GoogleUserProfile Auth_Google_Get_UserProfile(IEnumerable<Claim> claims)
        {
            var userId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var givenName = claims?.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var surname = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value;
            var profilePicture = claims?.FirstOrDefault(c => c.Type == "urn:google:picture")?.Value;

            GoogleUserProfile userProfile = new GoogleUserProfile
            {
                UserId = userId,
                Email = email,
                UserName = userName,
                GivenName = givenName,
                Surname = surname,
                ProfilePicture = profilePicture
            };

            return userProfile;
        }

        protected TokenContent Auth_Google_Generate_TokenContent(string provider, GoogleUserProfile userProfile)
        {            
            string jwtKey = _configuration["Authentication:Jwt:Key"] ?? string.Empty;
            string jwtIssuer = _configuration["Authentication:Jwt:Issuer"] ?? string.Empty;
            string jwtAudience = _configuration["Authentication:Jwt:Audience"] ?? string.Empty;
            double jwtExpiresInMinutes = _configuration.GetValue<double>("Authentication:Jwt:ExpiresInMinutes");
            DateTime jwtExpires = DateTime.UtcNow.AddMinutes(jwtExpiresInMinutes);

            // Create JWT token
            JwtContent jwtContent = new JwtContent
            {
                Key = jwtKey ?? string.Empty,
                Issuer = jwtIssuer ?? string.Empty,
                Audience = jwtAudience ?? string.Empty,
                Expires = jwtExpires,
                LoginProviderName = provider ?? string.Empty,
                LoginNameIdentifier = userProfile?.Email ?? string.Empty,
                Sid = userProfile?.UserId ?? string.Empty,
                UserNameIdentifier = userProfile?.Email ?? string.Empty,
                Email = userProfile?.Email ?? string.Empty,
                Name = userProfile?.UserName ?? string.Empty,
                GivenName = userProfile?.GivenName ?? string.Empty,
                Surname = userProfile?.Surname ?? string.Empty
            };
            
            return Auth_Generate_TokenContent(jwtContent);
        }

        protected TokenContent Auth_Standard_Generate_TokenContent(string provider, string loginName, SysUser user)
        {            
            string jwtKey = _configuration["Authentication:Jwt:Key"] ?? string.Empty;
            string jwtIssuer = _configuration["Authentication:Jwt:Issuer"] ?? string.Empty;
            string jwtAudience = _configuration["Authentication:Jwt:Audience"] ?? string.Empty;
            double jwtExpiresInMinutes = _configuration.GetValue<double>("Authentication:Jwt:ExpiresInMinutes");
            DateTime jwtExpires = DateTime.UtcNow.AddMinutes(jwtExpiresInMinutes);

            // Create JWT token
            JwtContent jwtContent = new JwtContent
            {
                Key = jwtKey ?? string.Empty,
                Issuer = jwtIssuer ?? string.Empty,
                Audience = jwtAudience ?? string.Empty,
                Expires = jwtExpires,
                LoginProviderName = provider ?? string.Empty,
                LoginNameIdentifier = loginName ?? string.Empty,
                Sid = user?.Id.ToString() ?? string.Empty,
                UserNameIdentifier = user?.UserName ?? string.Empty,
                Email = user?.Email ?? string.Empty,
                Name = user?.UserName ?? string.Empty,
                GivenName = user?.GivenName ?? string.Empty,
                Surname = user?.Surname ?? string.Empty
            };
            
            return Auth_Generate_TokenContent(jwtContent);
        }
    }
}