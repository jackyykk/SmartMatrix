using AutoMapper;
using SmartMatrix.Domain.Demos.WeatherForecastDemo.Entities;
using SmartMatrix.Domain.Demos.WeatherForecastDemo.Payloads;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class WeatherForecast_Profile : Profile
    {
        public WeatherForecast_Profile()
        {
            CreateMap<WeatherForecast, WeatherForecastPayload>().ReverseMap();            
        }
    }
}