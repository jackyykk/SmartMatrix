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
            CreateMap<TokenContent, SysLogin_TryRenewRefreshToken_Response>().ReverseMap();                        
        }
    }
}