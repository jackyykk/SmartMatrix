using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.Application.Interfaces.DataAccess.Transactions;
using SmartMatrix.DataAccess.DbConnections;
using SmartMatrix.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Repositories;
using SmartMatrix.DataAccess.Repositories.Core.Identities;
using SmartMatrix.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.DataAccess.Transactions;

namespace SmartMatrix.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {        
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<CoreReadDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CoreReadConnection")));
            services.AddDbContext<CoreWriteDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CoreWriteConnection")));
            services.AddDbContext<DemoReadDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DemoReadConnection")));
            services.AddDbContext<DemoWriteDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DemoWriteConnection")));
            
            services.AddScoped<ICoreReadDbContext>(provider => provider.GetService<CoreReadDbContext>()!);
            services.AddScoped<ICoreWriteDbContext>(provider => provider.GetService<CoreWriteDbContext>()!);
            services.AddScoped<IDemoReadDbContext>(provider => provider.GetService<DemoReadDbContext>()!);
            services.AddScoped<IDemoWriteDbContext>(provider => provider.GetService<DemoWriteDbContext>()!);

            // Register Repositories
            services.AddScoped(typeof(ICoreReadRepo<,>), typeof(CoreReadRepo<,>));
            services.AddScoped(typeof(ICoreWriteRepo<,>), typeof(CoreWriteRepo<,>));
            services.AddScoped<ISysUserRepo, SysUserRepo>();            
            services.AddScoped<ISysLoginRepo, SysLoginRepo>();

            services.AddScoped(typeof(IDemoReadRepo<,>), typeof(DemoReadRepo<,>));
            services.AddScoped(typeof(IDemoWriteRepo<,>), typeof(DemoWriteRepo<,>));
            services.AddScoped<ISimpleNoteRepo, SimpleNoteRepo>();

            // Register Connections
            services.AddScoped<ICoreWriteDbConnection, CoreWriteDbConnection>();
            services.AddScoped<ICoreReadDbConnection, CoreReadDbConnection>();

            services.AddScoped<IDemoWriteDbConnection, DemoWriteDbConnection>();
            services.AddScoped<IDemoReadDbConnection, DemoReadDbConnection>();

            // Unit Of Work
            services.AddScoped<ICoreUnitOfWork, CoreUnitOfWork>();
            services.AddScoped<IDemoUnitOfWork, DemoUnitOfWork>();

            return services;
        }
    }
}