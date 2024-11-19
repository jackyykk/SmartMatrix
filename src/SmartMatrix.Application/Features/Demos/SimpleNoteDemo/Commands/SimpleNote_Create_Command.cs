using System.Transactions;
using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.Application.Interfaces.DataAccess.Transactions;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries
{
    public class SimpleNote_Create_Command : IRequest<Result<SimpleNote_Create_Response>>
    {
        public SimpleNote_Create_Request? Request { get; set; }

        public class Handler : IRequestHandler<SimpleNote_Create_Command, Result<SimpleNote_Create_Response>>
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

            public async Task<Result<SimpleNote_Create_Response>> Handle(SimpleNote_Create_Command command, CancellationToken cancellationToken)
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

                        var mappedEntity = _mapper.Map<SimpleNote_Create_Response>(entity);
                        return Result<SimpleNote_Create_Response>.Success(mappedEntity);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Result<SimpleNote_Create_Response>.Fail(-1, ex.Message);
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