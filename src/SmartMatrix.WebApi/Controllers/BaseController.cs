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
    // Endpoint Instructions for Web API functions
    //
    // Controller should be properly categorized
    //    
    // Function Name should be in lower case
    //
    // {action}-{conditions}[optional]-{entity}[optional]
    //
    // {action}: e.g. insert, update, delete, get, getfirst, getlist, login, logout, etc.
    // {conditions}: e.g. by_id, by_name, by_email, by_xxx, etc.
    // {entity}: e.g. main_config
    //
    // Use - to separte the entity into category and sub-category
    // Use _ to connect the words to the same entity

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

        protected string GetErrorMessage(Exception ex)
        {
            string message = ex.Message + (ex.InnerException != null ? " || " + ex.InnerException.Message : "");
            return message;
        }

        protected SysSecret Auth_Get_AccessToken_Secret()
        {
            SysSecret secret;
            string accessToken_Format = _configuration["Authentication:AccessToken:Format"] ?? string.Empty;
            double accessToken_LifeInMinutes = _configuration.GetValue<double>("Authentication:AccessToken:LifeInMinutes");
            DateTime accessToken_Expires = DateTime.UtcNow.AddMinutes(accessToken_LifeInMinutes);

            if (string.Equals(accessToken_Format, "Jwt", System.StringComparison.InvariantCultureIgnoreCase))
            {
                string jwtKey = _configuration["Authentication:Jwt:Key"] ?? string.Empty;
                string jwtIssuer = _configuration["Authentication:Jwt:Issuer"] ?? string.Empty;
                string jwtAudience = _configuration["Authentication:Jwt:Audience"] ?? string.Empty;    

                // Create JWT token
                secret = new SysSecret
                {
                    Format = accessToken_Format,
                    Key = jwtKey ?? string.Empty,
                    Issuer = jwtIssuer ?? string.Empty,
                    Audience = jwtAudience ?? string.Empty,
                    LifeInMinutes = accessToken_LifeInMinutes,
                    Expires = accessToken_Expires,                
                };
            }
            else
            {
                throw new System.Exception("Invalid access token format");                
            }
                                            
            return secret;
        }

        protected SysToken Auth_Generate_SysToken(SysTokenContent content)
        {
            SysToken token = new SysToken();
            
            if (content.Secret == null || string.IsNullOrEmpty(content.Secret.Format))
            {
                throw new System.Exception("Invalid secret");
            }

            if (!string.Equals(content.Secret.Format, "Jwt", System.StringComparison.InvariantCultureIgnoreCase))
            {
                throw new System.Exception("Invalid access token format");
            }

            // Only support Jwt for now
            var accessToken = TokenUtil.EncodeJwt(content);
            var refreshToken = TokenUtil.GenerateRefreshToken();
            double refreshToken_LifeInMinutes = _configuration.GetValue<double>("Authentication:RefreshToken:LifeInMinutes");
            DateTime refreshToken_Expires = DateTime.UtcNow.AddMinutes(refreshToken_LifeInMinutes);

            token.AccessToken = accessToken;
            token.AccessToken_LifeInMinutes = content.Secret.LifeInMinutes;
            token.AccessToken_Expires = content.Secret.Expires;            
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

        protected SysToken Auth_Google_Generate_SysToken(string provider, GoogleUserProfile userProfile)
        {
            // Create JWT token
            SysSecret secret = Auth_Get_AccessToken_Secret();

            SysTokenContent content = new SysTokenContent
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
            
            return Auth_Generate_SysToken(content);
        }

        protected SysToken Auth_Standard_Generate_SysToken(string provider, string loginName, SysUser user)
        {            
            SysSecret secret = Auth_Get_AccessToken_Secret();

            SysTokenContent content = new SysTokenContent
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
                    
            return Auth_Generate_SysToken(content);
        }
    }
}