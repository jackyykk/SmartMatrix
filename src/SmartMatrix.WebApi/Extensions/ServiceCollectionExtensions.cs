using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Localization;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.WebApi.Localization;
using SmartMatrix.WebApi.Services;

namespace SmartMatrix.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();

            services.TryAddTransient(typeof(IStringLocalizer<>), typeof(ServerLocalizer<>));

            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUserService, CurrentUserService>();
            
            return services;
        }
    }
}