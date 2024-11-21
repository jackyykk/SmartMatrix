using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class SysUser_GetById_Query : IRequest<Result<SysUser_GetById_Response>>
    {
        public SysUser_GetById_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysUser_GetById_Query, Result<SysUser_GetById_Response>>
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
                try
                {
                    var entity = await _sysUserRepo.GetByIdAsync(query.Request!.PartitionKey, query.Request!.Id);
                    var mappedEntity = _mapper.Map<SysUser_GetById_Response>(entity);
                    
                    return Result<SysUser_GetById_Response>.Success(mappedEntity);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_GetById_Response>.Fail(-1, ex.Message);
                }                
            }
        }
    }
}