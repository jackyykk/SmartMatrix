using AutoMapper;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Payloads;
using SmartMatrix.Domain.Demos.WeatherForecastDemo.Entities;
using SmartMatrix.Domain.Demos.WeatherForecastDemo.Payloads;

namespace SmartMatrix.Application.Mappings
{    
    internal class DemoMappingProfile : Profile
    {
        public DemoMappingProfile()
        {
            CreateMap<SimpleNote, SimpleNotePayload>().ReverseMap();

            CreateMap<WeatherForecast, WeatherForecastPayload>().ReverseMap();            
        }
    }
}