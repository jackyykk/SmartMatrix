using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMatrix.Application.Interfaces.Services.Common;
using SmartMatrix.Infrastructure.Services.Common;

namespace SmartMatrix.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTimeService, SystemDateTimeService>();
            return services;
        }
    }
}