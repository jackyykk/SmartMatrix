using AutoMapper;
using SmartMatrix.Domain.Entities.Demos;
using SmartMatrix.Domain.Messages.Demos.SimpleNotes;

namespace SmartMatrix.Application.Mappings.Demos.SimpleNotes
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SimpleNote, GetSimpleNoteByIdRequest>().ReverseMap();            
        }
    }
}