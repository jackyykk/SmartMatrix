using AutoMapper;
using SmartMatrix.Domain.Tools.SimpleNoteTool.DbEntities;
using SmartMatrix.Domain.Tools.SimpleNoteTool.Payloads;
using SmartMatrix.Domain.Tools.WeatherForecastTool.Entities;
using SmartMatrix.Domain.Tools.WeatherForecastTool.Payloads;

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