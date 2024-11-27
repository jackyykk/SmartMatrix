using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysLogin_GetFirstByOneTimeToken_Query : IRequest<Result<SysLogin_GetFirstByOneTimeToken_Response>>
    {
        public SysLogin_GetFirstByOneTimeToken_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysLogin_GetFirstByOneTimeToken_Query, Result<SysLogin_GetFirstByOneTimeToken_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysLoginRepo _sysLoginRepo;
            
            public Handler(IMapper mapper, ISysLoginRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysLoginRepo = sysUserRepo;
            }

            public async Task<Result<SysLogin_GetFirstByOneTimeToken_Response>> Handle(SysLogin_GetFirstByOneTimeToken_Query query, CancellationToken cancellationToken)
            {
                SysLogin_GetFirstByOneTimeToken_Response response = new SysLogin_GetFirstByOneTimeToken_Response();

                if (query.Request == null)
                {
                    return Result<SysLogin_GetFirstByOneTimeToken_Response>.Fail(SysLogin_GetFirstByOneTimeToken_Response.StatusCodes.Invalid_Request, SysLogin_GetFirstByOneTimeToken_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                {
                    return Result<SysLogin_GetFirstByOneTimeToken_Response>.Fail(SysLogin_GetFirstByOneTimeToken_Response.StatusCodes.Invalid_Request, SysLogin_GetFirstByOneTimeToken_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.OneTimeToken))                        
                {
                    return Result<SysLogin_GetFirstByOneTimeToken_Response>.Fail(SysLogin_GetFirstByOneTimeToken_Response.StatusCodes.Invalid_Request, SysLogin_GetFirstByOneTimeToken_Response.StatusTexts.Invalid_Request);
                }

                try
                {                    
                    var existingLogin = await _sysLoginRepo.GetFirstByOneTimeTokenAsync(query.Request!.PartitionKey, query.Request!.OneTimeToken);
                    var outputLogin = _mapper.Map<SysLogin_OutputPayload>(existingLogin);
                    response.Login = outputLogin;
                    return Result<SysLogin_GetFirstByOneTimeToken_Response>.Success(response, SysLogin_GetFirstByOneTimeToken_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysLogin_GetFirstByOneTimeToken_Response>.Fail(SysLogin_GetFirstByOneTimeToken_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }                
            }
        }
    }
}