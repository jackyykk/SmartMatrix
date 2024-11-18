using AutoMapper;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SysUserProfile : Profile
    {
        public SysUserProfile()
        {               
            CreateMap<SysUser, GetFirstSysUserByUserNameResponse>().ReverseMap();            
            CreateMap<SysUser, GetFirstSysUserByLoginNameResponse>().ReverseMap();         
        }
    }
}