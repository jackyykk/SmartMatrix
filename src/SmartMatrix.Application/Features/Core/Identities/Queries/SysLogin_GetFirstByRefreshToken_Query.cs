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
                    if (query.Request == null)
                    {
                        return Result<SysLogin_GetFirstByRefreshToken_Response>.Fail(SysLogin_GetFirstByRefreshToken_Response.StatusCodes.Invalid_Request, SysLogin_GetFirstByRefreshToken_Response.StatusTexts.Invalid_Request);
                    }

                    if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                    {
                        return Result<SysLogin_GetFirstByRefreshToken_Response>.Fail(SysLogin_GetFirstByRefreshToken_Response.StatusCodes.Invalid_Request, SysLogin_GetFirstByRefreshToken_Response.StatusTexts.Invalid_Request);
                    }

                    if (string.IsNullOrEmpty(query.Request!.RefreshToken))                        
                    {
                        return Result<SysLogin_GetFirstByRefreshToken_Response>.Fail(SysLogin_GetFirstByRefreshToken_Response.StatusCodes.Invalid_Request, SysLogin_GetFirstByRefreshToken_Response.StatusTexts.Invalid_Request);
                    }

                    var entity = await _sysLoginRepo.GetFirstByRefreshTokenAsync(query.Request!.PartitionKey, query.Request!.RefreshToken);
                    var mappedEntity = _mapper.Map<SysLogin_GetFirstByRefreshToken_Response>(entity);
                    
                    return Result<SysLogin_GetFirstByRefreshToken_Response>.Success(mappedEntity, SysLogin_GetFirstByRefreshToken_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysLogin_GetFirstByRefreshToken_Response>.Fail(SysLogin_GetFirstByRefreshToken_Response.StatusCodes.Unknown_Error, ex.Message);
                }                
            }
        }
    }
}