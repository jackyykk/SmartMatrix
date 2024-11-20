using AutoMapper;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SysUser_Profile : Profile
    {
        public SysUser_Profile()
        {   
            CreateMap<SysUser, SysUser_PerformLogin_Response>().ReverseMap();            
            CreateMap<SysUser, SysUser_GetFirstByUserName_Response>().ReverseMap();
            CreateMap<SysUser, SysUser_GetFirstByLoginName_Response>().ReverseMap();         
        }
    }
}