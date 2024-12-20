using System;
using SmartMatrix.Domain.Tools.WeatherForecastTool.Entities;

namespace SmartMatrix.Domain.Tools.WeatherForecastTool.Payloads
{
    public class WeatherForecast_OutputPayload    
    {        
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}