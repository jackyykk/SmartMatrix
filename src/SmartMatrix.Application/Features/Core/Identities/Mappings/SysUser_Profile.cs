using AutoMapper;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SysUser_Profile : Profile
    {
        public SysUser_Profile()
        {
            CreateMap<SysUser, SysUserPayload>().ReverseMap();            
        }
    }
}