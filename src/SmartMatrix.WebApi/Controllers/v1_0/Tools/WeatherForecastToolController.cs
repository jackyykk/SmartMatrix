using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Tools.WeatherForecastTool.Entities;
using SmartMatrix.Domain.Tools.WeatherForecastTool.Messages;
using SmartMatrix.Domain.Tools.WeatherForecastTool.Payloads;

namespace SmartMatrix.WebApi.Controllers.v1_0.Tools
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tools/weather_forecast_tool")]
    [ApiVersion("1.0")]
    public class WeatherForecastToolController : BaseController<WeatherForecastToolController>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastToolController(ILogger<WeatherForecastToolController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
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