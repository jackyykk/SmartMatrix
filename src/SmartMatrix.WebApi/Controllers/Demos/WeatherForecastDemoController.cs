using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartMatrix.WebApi.Controllers.Demos
{
    [ApiController]
    [Route("api/demos/weather-forecast-demo")]
    public class WeatherForecastDemoController : BaseController<WeatherForecastDemoController>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastDemoController(ILogger<WeatherForecastDemoController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        [HttpGet("get-list")]
        public IEnumerable<WeatherForecast> GetList()
        {
            return Enumerable.Range(1, 100).Select(index => new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                Summaries[Random.Shared.Next(Summaries.Length)]
            ))
            .ToArray();
        }
    }

    public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);
}