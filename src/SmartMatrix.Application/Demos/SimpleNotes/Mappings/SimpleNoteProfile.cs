using AutoMapper;
using SmartMatrix.Domain.Demos.SimpleNotes.Entities;
using SmartMatrix.Domain.Demos.SimpleNotes.Messages;

namespace SmartMatrix.Application.Demos.SimpleNotes.Mappings
{
    internal class SimpleNoteProfile : Profile
    {
        public SimpleNoteProfile()
        {
            CreateMap<SimpleNote, GetSimpleNoteByIdResponse>().ReverseMap();                     
        }
    }
}