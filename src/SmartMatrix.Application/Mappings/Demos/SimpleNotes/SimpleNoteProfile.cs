using AutoMapper;
using SmartMatrix.Domain.Entities.Demos;
using SmartMatrix.Domain.Messages.Demos.SimpleNotes;

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