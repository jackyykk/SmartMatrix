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
            private readonly IMapper _mapper;
            private readonly ISimpleNoteRepo _simpleNoteRepo;
            private readonly IDemoUnitOfWork _unitOfWork;
            
            public Handler(IMapper mapper, ISimpleNoteRepo simpleNoteRepo, IDemoUnitOfWork unitOfWork)
            {
                _mapper = mapper;
                _simpleNoteRepo = simpleNoteRepo;
                _unitOfWork = unitOfWork;                                
            }

            public async Task<Result<SimpleNote_Create_Response>> Handle(SimpleNote_Create_Command command, CancellationToken cancellationToken)
            {
                if (command.Request == null)
                {
                    return Result<SimpleNote_Create_Response>.Fail(SimpleNote_Create_Response.StatusCodes.Invalid_Request, SimpleNote_Create_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.SimpleNote == null)                        
                {
                    return Result<SimpleNote_Create_Response>.Fail(SimpleNote_Create_Response.StatusCodes.Invalid_Request, SimpleNote_Create_Response.StatusTexts.Invalid_Request);
                }
                
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
                        return Result<SimpleNote_Create_Response>.Success(mappedEntity, SimpleNote_Create_Response.StatusCodes.Success);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Result<SimpleNote_Create_Response>.Fail(SimpleNote_Create_Response.StatusCodes.Unknown_Error, ex.Message);
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