using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Application.Interfaces.DataAccess.Transactions;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Application.Features.Core.Identities.Commands
{
    public class SysUser_CreateUser_Command : IRequest<Result<SysUser_CreateUser_Response>>
    {
        public SysUser_CreateUser_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysUser_CreateUser_Command, Result<SysUser_CreateUser_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;            
            private readonly ICoreUnitOfWork _unitOfWork;

            public Handler(IMapper mapper, ISysUserRepo sysUserRepo, ICoreUnitOfWork unitOfWork)
            {
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;                
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<SysUser_CreateUser_Response>> Handle(SysUser_CreateUser_Command command, CancellationToken cancellationToken)
            {
                SysUser_CreateUser_Response response = new SysUser_CreateUser_Response();
                SysUser newUser;

                if (command.Request == null)
                {
                    return Result<SysUser_CreateUser_Response>.Fail(SysUser_CreateUser_Response.StatusCodes.Invalid_Request, SysUser_CreateUser_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.User == null)
                {
                    return Result<SysUser_CreateUser_Response>.Fail(SysUser_CreateUser_Response.StatusCodes.Invalid_Request, SysUser_CreateUser_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.User.Id != 0)
                {
                    return Result<SysUser_CreateUser_Response>.Fail(SysUser_CreateUser_Response.StatusCodes.Invalid_Request, SysUser_CreateUser_Response.StatusTexts.Invalid_Request);
                }

                try
                {
                    _unitOfWork.Open();
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _sysUserRepo.SetTransaction(transaction);

                            var inputUser = command.Request!.User;

                            newUser = _mapper.Map<SysUser>(inputUser);

                            newUser = await _sysUserRepo.InsertAsync(newUser);
                            
                            await _unitOfWork.SaveChangesAsync(cancellationToken);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {                            
                            transaction.Rollback();                            
                            return Result<SysUser_CreateUser_Response>.Fail(SysUser_CreateUser_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                        }
                        finally
                        {
                            _unitOfWork.Close();
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    return Result<SysUser_CreateUser_Response>.Fail(SysUser_CreateUser_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }

                if (newUser != null)
                {
                    var existingUser = await _sysUserRepo.GetByIdAsync(newUser.PartitionKey, newUser.Id);
                    
                    var outputUser = _mapper.Map<SysUser_OutputPayload>(existingUser);                

                    response.User = outputUser;
                }
                                
                return Result<SysUser_CreateUser_Response>.Success(response, SysUser_CreateUser_Response.StatusCodes.Success);
            }
        }
    }
}