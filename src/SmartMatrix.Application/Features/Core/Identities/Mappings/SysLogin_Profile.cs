using AutoMapper;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SysLogin_Profile : Profile
    {
        public SysLogin_Profile()
        {            
            CreateMap<SysLogin, SysLoginPayload>().ReverseMap();
        }
    }
}