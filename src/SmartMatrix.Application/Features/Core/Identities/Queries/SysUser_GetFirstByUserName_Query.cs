using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysUser_GetFirstByUserName_Query : IRequest<Result<SysUser_GetFirstByUserName_Response>>
    {
        public SysUser_GetFirstByUserName_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysUser_GetFirstByUserName_Query, Result<SysUser_GetFirstByUserName_Response>>
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
                try
                {
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

                    var entity = await _sysUserRepo.GetFirstByUserNameAsync(query.Request!.PartitionKey, query.Request!.UserName);
                    var mappedEntity = _mapper.Map<SysUser_GetFirstByUserName_Response>(entity);
                    
                    return Result<SysUser_GetFirstByUserName_Response>.Success(mappedEntity, SysUser_GetFirstByUserName_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_GetFirstByUserName_Response>.Fail(SysUser_GetFirstByUserName_Response.StatusCodes.Unknown_Error, ex.Message);
                }                
            }
        }
    }
}