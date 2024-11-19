using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries
{
    public class SimpleNote_GetById_Query : IRequest<Result<SimpleNote_GetById_Response>>
    {
        public SimpleNote_GetById_Request? Request { get; set; }

        public class Handler : IRequestHandler<SimpleNote_GetById_Query, Result<SimpleNote_GetById_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISimpleNoteRepo _simpleNoteRepo;
            
            public Handler(IMapper mapper, ISimpleNoteRepo simpleNoteRepo)
            {                
                _mapper = mapper;
                _simpleNoteRepo = simpleNoteRepo;
            }

            public async Task<Result<SimpleNote_GetById_Response>> Handle(SimpleNote_GetById_Query query, CancellationToken cancellationToken)
            {                
                try
                {
                    var entity = await _simpleNoteRepo.GetByIdAsync(query.Request!.Id);
                    var mappedEntity = _mapper.Map<SimpleNote_GetById_Response>(entity);
                    
                    return Result<SimpleNote_GetById_Response>.Success(mappedEntity);
                }
                catch (Exception ex)
                {
                    return Result<SimpleNote_GetById_Response>.Fail(-1, ex.Message);
                }                
            }
        }
    }
}