using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysLogin_GetFirstByRefreshToken_Query : IRequest<Result<SysLogin_GetFirstByRefreshToken_Response>>
    {
        public SysLogin_GetFirstByRefreshToken_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysLogin_GetFirstByRefreshToken_Query, Result<SysLogin_GetFirstByRefreshToken_Response>>
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
                SysLogin_GetFirstByRefreshToken_Response response = new SysLogin_GetFirstByRefreshToken_Response();

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

                try
                {                    
                    var existingLogin = await _sysLoginRepo.GetFirstByRefreshTokenAsync(query.Request!.PartitionKey, query.Request!.RefreshToken);
                    var outputLogin = _mapper.Map<SysLogin_OutputPayload>(existingLogin);
                    response.Login = outputLogin;
                    return Result<SysLogin_GetFirstByRefreshToken_Response>.Success(response, SysLogin_GetFirstByRefreshToken_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysLogin_GetFirstByRefreshToken_Response>.Fail(SysLogin_GetFirstByRefreshToken_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }                
            }
        }
    }
}