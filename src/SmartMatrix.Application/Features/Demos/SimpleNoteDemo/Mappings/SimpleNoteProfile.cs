using AutoMapper;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Entities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SimpleNoteProfile : Profile
    {
        public SimpleNoteProfile()
        {
            CreateMap<SimpleNote, GetSimpleNoteByIdResponse>().ReverseMap();                     
        }
    }
}