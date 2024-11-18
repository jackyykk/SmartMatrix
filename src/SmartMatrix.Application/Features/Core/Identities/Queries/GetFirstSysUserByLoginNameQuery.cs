using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Queries
{
    public class GetFirstSysUserByLoginNameQuery : IRequest<Result<GetFirstSysUserByLoginNameResponse>>
    {
        public GetFirstSysUserByLoginNameRequest? Request { get; set; }

        public class Handler : IRequestHandler<GetFirstSysUserByLoginNameQuery, Result<GetFirstSysUserByLoginNameResponse>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<GetFirstSysUserByLoginNameResponse>> Handle(GetFirstSysUserByLoginNameQuery query, CancellationToken cancellationToken)
            {                
                try
                {
                    var entity = await _sysUserRepo.GetFirstByLoginNameAsync(query.Request!.LoginName);
                    var mappedEntity = _mapper.Map<GetFirstSysUserByLoginNameResponse>(entity);
                    
                    return Result<GetFirstSysUserByLoginNameResponse>.Success(mappedEntity);
                }
                catch (Exception ex)
                {
                    return Result<GetFirstSysUserByLoginNameResponse>.Fail(-1, ex.Message);
                }                
            }
        }
    }
}