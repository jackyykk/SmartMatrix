using System.Transactions;
using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.Application.Interfaces.DataAccess.Transactions;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries
{
    public class CreateSimpleNoteCommand : IRequest<Result<CreateSimpleNoteResponse>>
    {
        public CreateSimpleNoteRequest? Request { get; set; }

        public class Handler : IRequestHandler<CreateSimpleNoteCommand, Result<CreateSimpleNoteResponse>>
        {
            private readonly IDemoUnitOfWork _unitOfWork;
            private readonly ISimpleNoteRepo _simpleNoteRepo;
            private readonly IMapper _mapper;

            public Handler(IDemoUnitOfWork unitOfWork, ISimpleNoteRepo simpleNoteRepo, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _simpleNoteRepo = simpleNoteRepo;
                _mapper = mapper;
            }

            public async Task<Result<CreateSimpleNoteResponse>> Handle(CreateSimpleNoteCommand command, CancellationToken cancellationToken)
            {
                
                _unitOfWork.Open();
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        _simpleNoteRepo.SetTransaction(transaction);

                        var entity = await _simpleNoteRepo.InsertAsync(command.Request!.SimpleNote);
                        
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        transaction.Commit();

                        var mappedEntity = _mapper.Map<CreateSimpleNoteResponse>(entity);
                        return Result<CreateSimpleNoteResponse>.Success(mappedEntity);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Result<CreateSimpleNoteResponse>.Fail(-1, ex.Message);
                    }                    
                    finally
                    {
                        _unitOfWork.Close();
                    }
                }
            }
        }
    }
}