using System.Security.Claims;
using SmartMatrix.Application.Interfaces.Services.Essential;

namespace SmartMatrix.WebApi.Services
{
    public class CurrentUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string UserAccountId { get; }
        public string UserAccountName { get; }
        public string FullName { get; set; }
        public List<KeyValuePair<string, string>> Claims { get; set; } = new List<KeyValuePair<string, string>>();

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            UserAccountId  = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            UserAccountName = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name) ?? "";
            FullName = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName) ?? "";
        }
    }
}