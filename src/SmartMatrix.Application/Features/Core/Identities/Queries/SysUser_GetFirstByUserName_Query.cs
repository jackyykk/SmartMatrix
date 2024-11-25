using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysUser_GetFirstByUserName_Query : IRequest<Result<SysUser_GetFirstByUserName_Response>>
    {
        public SysUser_GetFirstByUserName_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysUser_GetFirstByUserName_Query, Result<SysUser_GetFirstByUserName_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<SysUser_GetFirstByUserName_Response>> Handle(SysUser_GetFirstByUserName_Query query, CancellationToken cancellationToken)
            {
                SysUser_GetFirstByUserName_Response response = new SysUser_GetFirstByUserName_Response();

                if (query.Request == null)
                {
                    return Result<SysUser_GetFirstByUserName_Response>.Fail(SysUser_GetFirstByUserName_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByUserName_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                {
                    return Result<SysUser_GetFirstByUserName_Response>.Fail(SysUser_GetFirstByUserName_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByUserName_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.UserName))                        
                {
                    return Result<SysUser_GetFirstByUserName_Response>.Fail(SysUser_GetFirstByUserName_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByUserName_Response.StatusTexts.Invalid_Request);
                }

                try
                {                    
                    var existingUser = await _sysUserRepo.GetFirstByUserNameAsync(query.Request!.PartitionKey, query.Request!.UserName);
                    
                    var outputUser = _mapper.Map<SysUser_OutputPayload>(existingUser);
                    if (existingUser != null && outputUser != null)
                    {
                        outputUser.Update_Roles_AuditInfo(existingUser);
                    }
                    
                    response.User = outputUser;

                    return Result<SysUser_GetFirstByUserName_Response>.Success(response, SysUser_GetFirstByUserName_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_GetFirstByUserName_Response>.Fail(SysUser_GetFirstByUserName_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }                
            }
        }
    }
}