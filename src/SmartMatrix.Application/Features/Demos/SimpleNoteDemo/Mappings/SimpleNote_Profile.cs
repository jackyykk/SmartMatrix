using AutoMapper;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Payloads;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SimpleNote_Profile : Profile
    {
        public SimpleNote_Profile()
        {
            CreateMap<SimpleNote, SimpleNotePayload>().ReverseMap();            
        }
    }
}