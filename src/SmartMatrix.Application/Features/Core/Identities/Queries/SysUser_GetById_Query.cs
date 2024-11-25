using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysUser_GetById_Query : IRequest<Result<SysUser_GetById_Response>>
    {
        public SysUser_GetById_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysUser_GetById_Query, Result<SysUser_GetById_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<SysUser_GetById_Response>> Handle(SysUser_GetById_Query query, CancellationToken cancellationToken)
            {
                SysUser_GetById_Response response = new SysUser_GetById_Response();

                if (query.Request == null)
                {
                    return Result<SysUser_GetById_Response>.Fail(SysUser_GetById_Response.StatusCodes.Invalid_Request, SysUser_GetById_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                {
                    return Result<SysUser_GetById_Response>.Fail(SysUser_GetById_Response.StatusCodes.Invalid_Request, SysUser_GetById_Response.StatusTexts.Invalid_Request);
                }

                if (query.Request!.Id <= 0)                        
                {
                    return Result<SysUser_GetById_Response>.Fail(SysUser_GetById_Response.StatusCodes.Invalid_Request, SysUser_GetById_Response.StatusTexts.Invalid_Request);
                }

                try
                {                                        
                    var existingUser = await _sysUserRepo.GetByIdAsync(query.Request!.PartitionKey, query.Request!.Id);
                    
                    var outputUser = _mapper.Map<SysUser_OutputPayload>(existingUser);
                    if (existingUser != null && outputUser != null)
                    {
                        outputUser.Update_Roles_AuditInfo(existingUser);
                    }

                    response.User = outputUser;

                    return Result<SysUser_GetById_Response>.Success(response, SysUser_GetById_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_GetById_Response>.Fail(SysUser_GetById_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }                
            }
        }
    }
}