using System.Collections.Generic;
using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Tools.WeatherForecastTool.Payloads;

namespace SmartMatrix.Domain.Tools.WeatherForecastTool.Messages
{
    public class WeatherForecast_GetList_Response
    {
        public static class StatusCodes
        {
            public const int Success = MessageConstants.StatusCodes.Success;
            public const int Unknown_Error = MessageConstants.StatusCodes.Unknown_Error;
            public const int Invalid_Request = MessageConstants.StatusCodes.Invalid_Request;                        
        }

        public static class StatusTexts
        {
            public const string Invalid_Request = MessageConstants.StatusTexts.Invalid_Request;                     
        }
        
        public List<WeatherForecast_OutputPayload> WeatherForecasts { get; set; } = new List<WeatherForecast_OutputPayload>();
    }
}