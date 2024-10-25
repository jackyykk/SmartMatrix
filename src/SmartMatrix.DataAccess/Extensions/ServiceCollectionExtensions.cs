using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNotes;
using SmartMatrix.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Repositories.Demos.SimpleNotes;

namespace SmartMatrix.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {        
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<DemoDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DemoConnection")));

            // Register repositories
            services.AddScoped<ISimpleNoteRepo, SimpleNoteRepo>();

            return services;
        }
    }
}