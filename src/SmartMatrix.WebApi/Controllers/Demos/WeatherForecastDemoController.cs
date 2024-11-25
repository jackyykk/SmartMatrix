using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Demos.WeatherForecastDemo.Entities;
using SmartMatrix.Domain.Demos.WeatherForecastDemo.Messages;
using SmartMatrix.Domain.Demos.WeatherForecastDemo.Payloads;

namespace SmartMatrix.WebApi.Controllers.Demos
{
    [ApiController]
    [Route("api/demos/weather_forecast_demo")]
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

        [HttpGet("getlist")]
        public async Task<IActionResult> GetList()
        {
            WeatherForecast_GetList_Response response = new WeatherForecast_GetList_Response();

            // Simulate delay
            await Task.Delay(100);

            var weatherForecasts = Enumerable.Range(1, 100).Select(index => new WeatherForecast
            {
                Date = DateTime.Today.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            var payloads = _mapper.Map<List<WeatherForecast_OutputPayload>>(weatherForecasts);
            response.WeatherForecasts = payloads;

            return Ok(Result<WeatherForecast_GetList_Response>.Success(response));
        }
    }    
}