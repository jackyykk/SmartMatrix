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

        public WeatherForecastDemoController(ILogger<WeatherForecastDemoController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
        {
        }

        [HttpGet("get-weather-forecasts", Name = "WeatherForecastDemoController.GetWeatherForecasts")]
        public IEnumerable<WeatherForecast> GetWeatherForecasts()
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