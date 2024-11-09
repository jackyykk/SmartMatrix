using AutoMapper;
using SmartMatrix.Domain.Demos.SimpleNotes.Entities;
using SmartMatrix.Domain.Demos.SimpleNotes.Messages;

namespace SmartMatrix.Application.Mappings.Demos.SimpleNotes
{
    public class SimpleNoteProfile : Profile
    {
        public SimpleNoteProfile()
        {
            CreateMap<SimpleNote, GetSimpleNoteByIdResponse>().ReverseMap();                     
        }
    }
}