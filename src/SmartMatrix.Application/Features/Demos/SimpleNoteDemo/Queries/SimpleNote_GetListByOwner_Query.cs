using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries
{
    public class SimpleNote_GetListByOwner_Query : IRequest<Result<List<SimpleNote_GetListByOwner_Response>>>
    {
        public SimpleNote_GetListByOwner_Request? Request { get; set; }

        public class Handler : IRequestHandler<SimpleNote_GetListByOwner_Query, Result<List<SimpleNote_GetListByOwner_Response>>>
        {
            private readonly IMapper _mapper;
            private readonly ISimpleNoteRepo _simpleNoteRepo;
            
            public Handler(IMapper mapper, ISimpleNoteRepo simpleNoteRepo)
            {                
                _mapper = mapper;
                _simpleNoteRepo = simpleNoteRepo;
            }

            public async Task<Result<List<SimpleNote_GetListByOwner_Response>>> Handle(SimpleNote_GetListByOwner_Query query, CancellationToken cancellationToken)
            {                
                try
                {
                    if (query.Request == null)
                    {
                        return Result<List<SimpleNote_GetListByOwner_Response>>.Fail(SimpleNote_GetListByOwner_Response.StatusCodes.Invalid_Request, SimpleNote_GetListByOwner_Response.StatusTexts.Invalid_Request);
                    }
                    
                    if (string.IsNullOrEmpty(query.Request!.Owner))                        
                    {
                        return Result<List<SimpleNote_GetListByOwner_Response>>.Fail(SimpleNote_GetListByOwner_Response.StatusCodes.Invalid_Request, SimpleNote_GetListByOwner_Response.StatusTexts.Invalid_Request);
                    }

                    var entity = await _simpleNoteRepo.GetListAsync(query.Request!.Owner);
                    var mappedEntity = _mapper.Map<List<SimpleNote_GetListByOwner_Response>>(entity);
                    
                    return Result<List<SimpleNote_GetListByOwner_Response>>.Success(mappedEntity, SimpleNote_GetListByOwner_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<List<SimpleNote_GetListByOwner_Response>>.Fail(SimpleNote_GetById_Response.StatusCodes.Unknown_Error, ex.Message);
                }                
            }
        }
    }
}