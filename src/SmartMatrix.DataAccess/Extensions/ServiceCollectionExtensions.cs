using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNotes;
using SmartMatrix.DataAccess.DbConnections;
using SmartMatrix.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Repositories;
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
            
            services.AddScoped<IDemoDbContext>(provider => provider.GetService<DemoDbContext>()!);

            // Register Repositories
            services.AddTransient(typeof(IDemoRepo<,>), typeof(DemoRepo<,>));
            services.AddScoped<ISimpleNoteRepo, SimpleNoteRepo>();

            // Register Connections
            services.AddScoped<IDemoWriteDbConnection, DemoWriteDbConnection>();
            services.AddScoped<IDemoReadDbConnection, DemoReadDbConnection>();

            return services;
        }
    }
}