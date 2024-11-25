using AutoMapper;    
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Mappings
{    
    internal class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<SysUser, SysUser_InputPayload>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));
            
            CreateMap<SysUser_InputPayload, SysUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.Roles.Select(r => new SysUserRole { SysUserId = src.Id, SysRoleId = r.Id })))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));            
            
            CreateMap<SysUser, SysUser_OutputPayload>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));
                    
            CreateMap<SysUser_OutputPayload, SysUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.Roles.Select(r => new SysUserRole { SysUserId = src.Id, SysRoleId = r.Id })))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));


            CreateMap<SysLogin, SysLogin_InputPayload>().ReverseMap();            
            CreateMap<SysLogin, SysLogin_OutputPayload>().ReverseMap();
            CreateMap<SysLogin_InputPayload, SysLogin_OutputPayload>().ReverseMap();

            CreateMap<SysRole, SysRole_InputPayload>().ReverseMap();
            CreateMap<SysRole, SysRole_OutputPayload>().ReverseMap();
            CreateMap<SysRole_InputPayload, SysRole_OutputPayload>().ReverseMap();

            CreateMap<SysToken, SysToken_OutputPayload>().ReverseMap();
        }
    }
}