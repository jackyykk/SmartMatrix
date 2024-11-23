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
    public class SysUser_InsertUser_Command : IRequest<Result<SysUser_InsertUser_Response>>
    {
        public SysUser_InsertUser_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysUser_InsertUser_Command, Result<SysUser_InsertUser_Response>>
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

            public async Task<Result<SysUser_InsertUser_Response>> Handle(SysUser_InsertUser_Command command, CancellationToken cancellationToken)
            {
                SysUser_InsertUser_Response response = new SysUser_InsertUser_Response();
                SysUser user;

                if (command.Request == null)
                {
                    return Result<SysUser_InsertUser_Response>.Fail(SysUser_InsertUser_Response.StatusCodes.Invalid_Request, SysUser_InsertUser_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.User == null)
                {
                    return Result<SysUser_InsertUser_Response>.Fail(SysUser_InsertUser_Response.StatusCodes.Invalid_Request, SysUser_InsertUser_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.User.Id != 0)
                {
                    return Result<SysUser_InsertUser_Response>.Fail(SysUser_InsertUser_Response.StatusCodes.Invalid_Request, SysUser_InsertUser_Response.StatusTexts.Invalid_Request);
                }

                try
                {
                    _unitOfWork.Open();
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _sysUserRepo.SetTransaction(transaction);

                            var userPayload = command.Request!.User;

                            user = _mapper.Map<SysUser>(userPayload);

                            user = await _sysUserRepo.InsertAsync(user);
                            
                            await _unitOfWork.SaveChangesAsync(cancellationToken);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {                            
                            transaction.Rollback();                            
                            return Result<SysUser_InsertUser_Response>.Fail(SysUser_InsertUser_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                        }
                        finally
                        {
                            _unitOfWork.Close();
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    return Result<SysUser_InsertUser_Response>.Fail(SysUser_InsertUser_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }

                if (user != null)
                {
                    response.User = _mapper.Map<SysUserPayload>(user);
                }
                                
                return Result<SysUser_InsertUser_Response>.Success(response, SysUser_InsertUser_Response.StatusCodes.Success);
            }
        }
    }
}