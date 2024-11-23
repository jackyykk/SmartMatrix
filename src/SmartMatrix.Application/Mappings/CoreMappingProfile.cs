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
            
            CreateMap<SysUser, SysUserPayload>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));
                    
            CreateMap<SysUserPayload, SysUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.Roles.Select(r => new SysUserRole { SysUserId = src.Id, SysRoleId = r.Id })))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));
                        
            CreateMap<SysLogin, SysLoginPayload>().ReverseMap();
            CreateMap<SysRole, SysRolePayload>().ReverseMap();
            CreateMap<SysToken, SysTokenPayload>().ReverseMap();
        }
    }
}