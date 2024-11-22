using AutoMapper;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SysToken_Profile : Profile
    {
        public SysToken_Profile()
        {
            CreateMap<SysToken, SysTokenPayload>().ReverseMap();            
        }
    }
}