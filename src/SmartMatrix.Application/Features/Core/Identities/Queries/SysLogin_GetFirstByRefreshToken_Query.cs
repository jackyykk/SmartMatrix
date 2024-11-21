using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysLogin_GetFirstByRefreshToken_Query : IRequest<Result<SysLogin_GetFirstByRefreshToken_Response>>
    {
        public SysLogin_GetFirstByRefreshToken_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysLogin_GetFirstByRefreshToken_Query, Result<SysLogin_GetFirstByRefreshToken_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysLoginRepo _sysLoginRepo;
            
            public Handler(IMapper mapper, ISysLoginRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysLoginRepo = sysUserRepo;
            }

            public async Task<Result<SysLogin_GetFirstByRefreshToken_Response>> Handle(SysLogin_GetFirstByRefreshToken_Query query, CancellationToken cancellationToken)
            {                
                try
                {
                    var entity = await _sysLoginRepo.GetFirstByRefreshTokenAsync(query.Request!.PartitionKey, query.Request!.RefreshToken);
                    var mappedEntity = _mapper.Map<SysLogin_GetFirstByRefreshToken_Response>(entity);
                    
                    return Result<SysLogin_GetFirstByRefreshToken_Response>.Success(mappedEntity);
                }
                catch (Exception ex)
                {
                    return Result<SysLogin_GetFirstByRefreshToken_Response>.Fail(-1, ex.Message);
                }                
            }
        }
    }
}