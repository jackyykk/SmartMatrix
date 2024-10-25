using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartMatrix.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);            
            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}