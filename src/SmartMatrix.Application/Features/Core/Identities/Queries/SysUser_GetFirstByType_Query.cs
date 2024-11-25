using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysUser_GetFirstByType_Query : IRequest<Result<SysUser_GetFirstByType_Response>>
    {
        public SysUser_GetFirstByType_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysUser_GetFirstByType_Query, Result<SysUser_GetFirstByType_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<SysUser_GetFirstByType_Response>> Handle(SysUser_GetFirstByType_Query query, CancellationToken cancellationToken)
            {
                SysUser_GetFirstByType_Response response = new SysUser_GetFirstByType_Response();

                if (query.Request == null)
                {
                    return Result<SysUser_GetFirstByType_Response>.Fail(SysUser_GetFirstByType_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByUserName_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                {
                    return Result<SysUser_GetFirstByType_Response>.Fail(SysUser_GetFirstByType_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByUserName_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.Type))                        
                {
                    return Result<SysUser_GetFirstByType_Response>.Fail(SysUser_GetFirstByType_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByUserName_Response.StatusTexts.Invalid_Request);
                }

                try
                {                    
                    var entity = await _sysUserRepo.GetFirstByTypeAsync(query.Request!.PartitionKey, query.Request!.Type);
                    var payload = _mapper.Map<SysUser_OutputPayload>(entity);
                    response.User = payload;
                    return Result<SysUser_GetFirstByType_Response>.Success(response, SysUser_GetFirstByType_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_GetFirstByType_Response>.Fail(SysUser_GetFirstByType_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }                
            }
        }
    }
}