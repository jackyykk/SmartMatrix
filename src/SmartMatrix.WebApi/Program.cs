using AutoMapper;
using SmartMatrix.Domain.Extensions;
using SmartMatrix.DataAccess.Extensions;
using SmartMatrix.Infrastructure.Extensions;
using SmartMatrix.WebApi.Extensions;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Kestrel to listen on a specific port
        // builder.WebHost.UseUrls("http://localhost:9001;https://localhost:9000");

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(); // Add Swagger services

        // Add CORS services and configure the policy to allow any origin
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

        // Add configuration to read from appsettings.json
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        // Register services from other projects
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddDataAccessServices(builder.Configuration);
        builder.Services.AddWebApiServices(builder.Configuration);

        // Enable AutoMapper diagnostics
        var serviceProvider = builder.Services.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerUI(); // Enable middleware to serve Swagger UI.
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // Use the CORS policy
        app.UseCors("AllowAllOrigins");

        // Serve static files and default files (index.html)
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapControllers();

        app.Run();
    }
}