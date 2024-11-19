using AutoMapper;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SimpleNote_Profile : Profile
    {
        public SimpleNote_Profile()
        {
            CreateMap<SimpleNote, SimpleNote_GetById_Response>().ReverseMap();
            CreateMap<SimpleNote, SimpleNote_Create_Response>().ReverseMap();
        }
    }
}