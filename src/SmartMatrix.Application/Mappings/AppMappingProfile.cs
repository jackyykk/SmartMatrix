using AutoMapper;
using SmartMatrix.Domain.Apps.SimpleNoteApp.DbEntities;
using SmartMatrix.Domain.Apps.SimpleNoteApp.Payloads;
using SmartMatrix.Domain.Apps.WeatherForecastApp.Entities;
using SmartMatrix.Domain.Apps.WeatherForecastApp.Payloads;

namespace SmartMatrix.Application.Mappings
{    
    internal class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<SimpleNote, SimpleNote_InputPayload>().ReverseMap();
            CreateMap<SimpleNote, SimpleNote_OutputPayload>().ReverseMap();

            CreateMap<WeatherForecast, WeatherForecast_OutputPayload>().ReverseMap();            
        }
    }
}