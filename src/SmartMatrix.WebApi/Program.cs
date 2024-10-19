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

        app.MapControllers();

        app.Run();
    }
}