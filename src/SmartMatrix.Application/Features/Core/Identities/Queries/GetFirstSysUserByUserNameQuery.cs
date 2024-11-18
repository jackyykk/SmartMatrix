using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class GetFirstSysUserByUserNameQuery : IRequest<Result<GetFirstSysUserByUserNameResponse>>
    {
        public GetFirstSysUserByUserNameRequest? Request { get; set; }

        public class Handler : IRequestHandler<GetFirstSysUserByUserNameQuery, Result<GetFirstSysUserByUserNameResponse>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<GetFirstSysUserByUserNameResponse>> Handle(GetFirstSysUserByUserNameQuery query, CancellationToken cancellationToken)
            {                
                try
                {
                    var entity = await _sysUserRepo.GetFirstByUserNameAsync(query.Request!.UserName);
                    var mappedEntity = _mapper.Map<GetFirstSysUserByUserNameResponse>(entity);
                    
                    return Result<GetFirstSysUserByUserNameResponse>.Success(mappedEntity);
                }
                catch (Exception ex)
                {
                    return Result<GetFirstSysUserByUserNameResponse>.Fail(-1, ex.Message);
                }                
            }
        }
    }
}