using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysUser_GetFirstByLoginName_Query : IRequest<Result<SysUser_GetFirstByLoginName_Response>>
    {
        public SysUser_GetFirstByLoginName_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysUser_GetFirstByLoginName_Query, Result<SysUser_GetFirstByLoginName_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<SysUser_GetFirstByLoginName_Response>> Handle(SysUser_GetFirstByLoginName_Query query, CancellationToken cancellationToken)
            {                
                try
                {
                    if (query.Request == null)
                    {
                        return Result<SysUser_GetFirstByLoginName_Response>.Fail(SysUser_GetFirstByLoginName_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByLoginName_Response.StatusTexts.Invalid_Request);
                    }

                    if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                    {
                        return Result<SysUser_GetFirstByLoginName_Response>.Fail(SysUser_GetFirstByLoginName_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByLoginName_Response.StatusTexts.Invalid_Request);
                    }

                    if (string.IsNullOrEmpty(query.Request!.LoginName))                        
                    {
                        return Result<SysUser_GetFirstByLoginName_Response>.Fail(SysUser_GetFirstByLoginName_Response.StatusCodes.Invalid_Request, SysUser_GetFirstByLoginName_Response.StatusTexts.Invalid_Request);
                    }

                    var entity = await _sysUserRepo.GetFirstByLoginNameAsync(query.Request!.PartitionKey, query.Request!.LoginName);
                    var mappedEntity = _mapper.Map<SysUser_GetFirstByLoginName_Response>(entity);
                    
                    return Result<SysUser_GetFirstByLoginName_Response>.Success(mappedEntity, SysUser_GetFirstByLoginName_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_GetFirstByLoginName_Response>.Fail(SysUser_GetFirstByLoginName_Response.StatusCodes.Unknown_Error, ex.Message);
                }                
            }
        }
    }
}