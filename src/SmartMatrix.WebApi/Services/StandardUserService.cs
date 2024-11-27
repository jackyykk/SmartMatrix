using System.Security.Claims;
using SmartMatrix.Application.Interfaces.Services.Essential;
using static SmartMatrix.Domain.Constants.CommonConstants.Identities;

namespace SmartMatrix.WebApi.Services
{
    public class StandardUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _accessor;

        public string LoginProviderName { get; }
        public string LoginNameIdentifier { get; }
        public string UserNameIdentifier { get; }
        public string Email { get; }
        public string Name { get; }
        public string GivenName { get; }
        public string Surname { get; }

        public List<KeyValuePair<string, string>> Claims { get; set; } = new List<KeyValuePair<string, string>>();

        public StandardUserService(IHttpContextAccessor httpContextAccessor)
        {
            _accessor = httpContextAccessor;

            LoginProviderName  = _accessor.HttpContext?.User?.FindFirstValue(SysClaimTypes.LoginProviderName) ?? "";
            LoginNameIdentifier = _accessor.HttpContext?.User?.FindFirstValue(SysClaimTypes.LoginNameIdentifier) ?? "";
            UserNameIdentifier = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            Email = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email) ?? "";
            Name = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "";
            GivenName = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName) ?? "";
            Surname = _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname) ?? "";

            Claims = _accessor.HttpContext?.User?.Claims?.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList() ?? new List<KeyValuePair<string, string>>();
        }
    }
}