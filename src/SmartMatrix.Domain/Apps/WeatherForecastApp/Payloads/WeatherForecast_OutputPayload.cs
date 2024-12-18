using System;
using SmartMatrix.Domain.Apps.WeatherForecastApp.Entities;

namespace SmartMatrix.Domain.Apps.WeatherForecastApp.Payloads
{
    public class WeatherForecast_OutputPayload    
    {        
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}