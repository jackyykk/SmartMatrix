using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysUser_GetFirstByClassificationAndType_Query : IRequest<Result<SysUser_GetFirstByClassificationAndType_Response>>
    {
        public SysUser_GetFirstByClassificationAndType_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysUser_GetFirstByClassificationAndType_Query, Result<SysUser_GetFirstByClassificationAndType_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<SysUser_GetFirstByClassificationAndType_Response>> Handle(SysUser_GetFirstByClassificationAndType_Query query, CancellationToken cancellationToken)
            {
                SysUser_GetFirstByClassificationAndType_Response response = new SysUser_GetFirstByClassificationAndType_Response();

                if (query.Request == null)
                {
                    return Result<SysUser_GetFirstByClassificationAndType_Response>.Fail(SysUser_GetFirstByClassificationAndType_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByClassificationAndType_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                {
                    return Result<SysUser_GetFirstByClassificationAndType_Response>.Fail(SysUser_GetFirstByClassificationAndType_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByClassificationAndType_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.Classification))                        
                {
                    return Result<SysUser_GetFirstByClassificationAndType_Response>.Fail(SysUser_GetFirstByClassificationAndType_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByClassificationAndType_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.Type))                        
                {
                    return Result<SysUser_GetFirstByClassificationAndType_Response>.Fail(SysUser_GetFirstByClassificationAndType_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByClassificationAndType_Response.StatusTexts.Invalid_Request);
                }

                try
                {                    
                    var existingUser = await _sysUserRepo.GetFirstByClassificationAndTypeAsync(query.Request!.PartitionKey, query.Request!.Classification, query.Request!.Type);
                    
                    var outputUser = _mapper.Map<SysUser_OutputPayload>(existingUser);                    

                    response.User = outputUser;

                    return Result<SysUser_GetFirstByClassificationAndType_Response>.Success(response, SysUser_GetFirstByClassificationAndType_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_GetFirstByClassificationAndType_Response>.Fail(SysUser_GetFirstByClassificationAndType_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }                
            }
        }
    }
}