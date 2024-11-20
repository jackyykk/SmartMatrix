using System.Security.Claims;
using AutoMapper;
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
        protected readonly IMapper _mapper;

        protected BaseController(ILogger<T> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;            
            _mediator = mediator;
            _mapper = mapper;
        }

        protected JwtSecret Auth_Get_JwtSecret()
        {
            string jwtKey = _configuration["Authentication:Jwt:Key"] ?? string.Empty;
            string jwtIssuer = _configuration["Authentication:Jwt:Issuer"] ?? string.Empty;
            string jwtAudience = _configuration["Authentication:Jwt:Audience"] ?? string.Empty;
            double jwtLifeInMinutes = _configuration.GetValue<double>("Authentication:Jwt:LifeInMinutes");
            DateTime jwtExpires = DateTime.UtcNow.AddMinutes(jwtLifeInMinutes);

            // Create JWT token
            JwtSecret secret = new JwtSecret
            {
                Key = jwtKey ?? string.Empty,
                Issuer = jwtIssuer ?? string.Empty,
                Audience = jwtAudience ?? string.Empty,
                LifeInMinutes = jwtLifeInMinutes,
                Expires = jwtExpires,                
            };

            return secret;
        }

        protected TokenContent Auth_Generate_TokenContent(JwtContent jwtContent)
        {
            TokenContent token = new TokenContent();

            var authToken = TokenUtil.EncodeJwt(jwtContent);
            var refreshToken = TokenUtil.GenerateRefreshToken();
            double refreshToken_LifeInMinutes = _configuration.GetValue<double>("Authentication:RefreshToken:LifeInMinutes");
            DateTime refreshToken_Expires = DateTime.UtcNow.AddMinutes(refreshToken_LifeInMinutes);

            token.AuthToken = authToken;
            token.AuthToken_LifeInMinutes = jwtContent.Secret.LifeInMinutes;
            token.AuthToken_Expires = jwtContent.Secret.Expires;            
            token.RefreshToken = refreshToken;
            token.RefreshToken_LifeInMinutes = refreshToken_LifeInMinutes;
            token.RefreshToken_Expires = refreshToken_Expires;

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
            // Create JWT token
            JwtSecret secret = Auth_Get_JwtSecret();

            JwtContent jwtContent = new JwtContent
            {
                Secret = secret,
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
            JwtSecret secret = Auth_Get_JwtSecret();

            JwtContent jwtContent = new JwtContent
            {
                Secret = secret,
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