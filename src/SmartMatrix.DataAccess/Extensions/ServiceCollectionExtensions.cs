using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Apps.SimpleNoteApp;
using SmartMatrix.Application.Interfaces.DataAccess.Transactions;
using SmartMatrix.DataAccess.DbConnections;
using SmartMatrix.DataAccess.DbContexts;
using SmartMatrix.DataAccess.Repositories;
using SmartMatrix.DataAccess.Repositories.Core.Identities;
using SmartMatrix.DataAccess.Repositories.Apps.SimpleNoteApp;
using SmartMatrix.DataAccess.Transactions;

namespace SmartMatrix.DataAccess.Extensions
{
    public static class ServiceCollectionExtensions
    {        
        public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<CoreReadDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CoreReadConnection")
                , options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            services.AddDbContext<CoreWriteDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CoreWriteConnection")
                , options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            services.AddDbContext<AppReadDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppReadConnection")
                , options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            services.AddDbContext<AppWriteDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AppWriteConnection")
                , options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            
            services.AddScoped<ICoreReadDbContext>(provider => provider.GetService<CoreReadDbContext>()!);
            services.AddScoped<ICoreWriteDbContext>(provider => provider.GetService<CoreWriteDbContext>()!);
            services.AddScoped<IAppReadDbContext>(provider => provider.GetService<AppReadDbContext>()!);
            services.AddScoped<IAppWriteDbContext>(provider => provider.GetService<AppWriteDbContext>()!);

            // Register Repositories
            services.AddScoped(typeof(ICoreReadRepo<,>), typeof(CoreReadRepo<,>));
            services.AddScoped(typeof(ICoreWriteRepo<,>), typeof(CoreWriteRepo<,>));
            services.AddScoped<ISysUserRepo, SysUserRepo>();            
            services.AddScoped<ISysLoginRepo, SysLoginRepo>();

            services.AddScoped(typeof(IAppReadRepo<,>), typeof(AppReadRepo<,>));
            services.AddScoped(typeof(IAppWriteRepo<,>), typeof(AppWriteRepo<,>));
            services.AddScoped<ISimpleNoteRepo, SimpleNoteRepo>();

            // Register Connections
            services.AddScoped<ICoreWriteDbConnection, CoreWriteDbConnection>();
            services.AddScoped<ICoreReadDbConnection, CoreReadDbConnection>();

            services.AddScoped<IAppWriteDbConnection, AppWriteDbConnection>();
            services.AddScoped<IAppReadDbConnection, AppReadDbConnection>();

            // Unit Of Work
            services.AddScoped<ICoreUnitOfWork, CoreUnitOfWork>();
            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();

            return services;
        }
    }
}