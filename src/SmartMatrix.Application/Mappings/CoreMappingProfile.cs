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
            // For genearating SysUser_InputPayload, it will map SysRole Of SysUserRole to SysRole directly
            CreateMap<SysUser, SysUser_InputPayload>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));
            
            // Since SysUser_InputPayload is mainly used for insert SysUser to system,
            // For generating SysUser, it will map SysUserId and SysRoleId only to SysUserRole
            CreateMap<SysUser_InputPayload, SysUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.Roles.Select(r => new SysUserRole { SysUserId = src.Id, SysRoleId = r.Id })))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));            
            
            // For generating SysRole_OutputPayload, it will try to update status of SysRole from SysUserRole before mapping
            CreateMap<SysUser, SysUser_OutputPayload>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.UpdateStatus_From(ur))))
                .ForMember(dest => dest.Logins, opt => opt.MapFrom(src => src.Logins));

            // For generating SysUserRole, it will map SysRole_OutputPayload of Roles to SysRole
            CreateMap<SysUser_OutputPayload, SysUser>()
                .ForMember(dest => dest.UserRoles, opt => opt.MapFrom(src => src.Roles.Select(r => new SysUserRole { SysUserId = src.Id, SysRoleId = r.Id, Role = r.ToSysRole() })))
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