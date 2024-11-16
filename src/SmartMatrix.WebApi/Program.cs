using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Google;
using SmartMatrix.Application.Extensions;
using SmartMatrix.DataAccess.Extensions;
using SmartMatrix.Infrastructure.Extensions;
using SmartMatrix.WebApi.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;

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
        var env = builder.Environment;
        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)            
            .AddEnvironmentVariables();

        // Register services from other projects
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddDataAccessServices(builder.Configuration);
        builder.Services.AddWebApiServices(builder.Configuration);

        // Configure Google Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/Auth/google/login"; // Redirect here if not authenticated
        })        
        .AddGoogle(options =>
        {
            options.ClientId = builder.Configuration["Google:ClientId"] ?? string.Empty;
            options.ClientSecret = builder.Configuration["Google:ClientSecret"] ?? string.Empty;            
            options.CallbackPath = "/Auth/google/callback";            
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.SaveTokens = true;
            options.Scope.Add("email"); // Include the email scope
            options.Scope.Add("profile"); // Optionally include profile scope for more user information

            // Optionally handle events
            options.Events = new OAuthEvents
            {
                OnCreatingTicket = context =>
                {
                    // Handle the event when creating the authentication ticket
                    return Task.CompletedTask;
                },
                OnRemoteFailure = context =>
                {
                    // Log the error details
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(context.Failure, "Remote authentication failure");

                    // Optionally, you can return detailed error information to the client
                    context.Response.Redirect($"/Home/Error?message={context.Failure?.Message}");
                    context.HandleResponse();
                    return Task.CompletedTask;
                }
            };
        });

/*
        builder.Services.AddAuthentication(options =>
        {            
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })        
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
            };
        });        
*/

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger(); // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwaggerUI(); // Enable middleware to serve Swagger UI.
        }

        // Enable AutoMapper Diagnostics        
        var mapper = app.Services.GetRequiredService<IMapper>();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        app.UseHttpsRedirection();

        // Use the CORS policy
        app.UseCors("AllowAllOrigins");

        app.UseAuthentication();
        app.UseAuthorization();
                
        // Serve static files and default files (index.html)
        app.UseDefaultFiles();
        app.UseStaticFiles();                

        app.MapControllers();
        

        app.Run();
    }
}