using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries
{
    public class GetSimpleNoteByIdQuery : IRequest<Result<GetSimpleNoteByIdResponse>>
    {
        public GetSimpleNoteByIdRequest? Request { get; set; }

        public class Handler : IRequestHandler<GetSimpleNoteByIdQuery, Result<GetSimpleNoteByIdResponse>>
        {
            private readonly IMapper _mapper;
            private readonly ISimpleNoteRepo _simpleNoteRepo;
            
            public Handler(IMapper mapper, ISimpleNoteRepo simpleNoteRepo)
            {                
                _mapper = mapper;
                _simpleNoteRepo = simpleNoteRepo;
            }

            public async Task<Result<GetSimpleNoteByIdResponse>> Handle(GetSimpleNoteByIdQuery query, CancellationToken cancellationToken)
            {                
                try
                {
                    var entity = await _simpleNoteRepo.GetByIdAsync(query.Request!.Id);
                    var mappedEntity = _mapper.Map<GetSimpleNoteByIdResponse>(entity);
                    
                    return Result<GetSimpleNoteByIdResponse>.Success(mappedEntity);
                }
                catch (Exception ex)
                {
                    return Result<GetSimpleNoteByIdResponse>.Fail(-1, ex.Message);
                }                
            }
        }
    }
}