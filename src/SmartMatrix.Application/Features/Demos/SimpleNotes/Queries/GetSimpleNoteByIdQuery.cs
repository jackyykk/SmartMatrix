using System.Transactions;
using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNotes;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Demos.SimpleNotes.Messages;


namespace SmartMatrix.Application.Features.Demos.SimpleNotes.Queries
{
    public class GetSimpleNoteByIdQuery : IRequest<Result<GetSimpleNoteByIdResponse>>
    {
        public GetSimpleNoteByIdRequest? Request { get; set; }

        public class Handler : IRequestHandler<GetSimpleNoteByIdQuery, Result<GetSimpleNoteByIdResponse>>
        {
            private readonly ISimpleNoteRepo _simpleNoteRepo;
            private readonly IMapper _mapper;

            public Handler(ISimpleNoteRepo simpleNoteRepo, IMapper mapper)
            {
                _simpleNoteRepo = simpleNoteRepo;
                _mapper = mapper;
            }

            public async Task<Result<GetSimpleNoteByIdResponse>> Handle(GetSimpleNoteByIdQuery query, CancellationToken cancellationToken)
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        var entity = await _simpleNoteRepo.GetByIdAsync(query.Request!.Id);
                        var mappedEntity = _mapper.Map<GetSimpleNoteByIdResponse>(entity);
                        scope.Complete();
                        return Result<GetSimpleNoteByIdResponse>.Success(mappedEntity);
                    }
                    catch (Exception ex)
                    {
                        return Result<GetSimpleNoteByIdResponse>.Fail(ex.Message);
                    }
                }
            }
        }
    }
}