using System.Reflection;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartMatrix.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register AutoMapper by providing the assembly that contains the mapping profiles
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register MediatR by providing the assembly that contains the handlers            
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}