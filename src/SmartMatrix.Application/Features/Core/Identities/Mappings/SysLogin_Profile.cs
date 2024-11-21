using AutoMapper;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Mappings
{
    internal class SysLogin_Profile : Profile
    {
        public SysLogin_Profile()
        {
            CreateMap<SysToken, SysLogin_RenewToken_Response>().ReverseMap();
            CreateMap<SysLogin, SysLogin_GetFirstByRefreshToken_Response>().ReverseMap();
        }
    }
}