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
    public class SysLogin_InsertLogin_Command : IRequest<Result<SysLogin_InsertLogin_Response>>
    {
        public SysLogin_InsertLogin_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysLogin_InsertLogin_Command, Result<SysLogin_InsertLogin_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysLoginRepo _sysLoginRepo;
            private readonly ICoreUnitOfWork _unitOfWork;

            public Handler(IMapper mapper, ISysLoginRepo sysUserRepo, ICoreUnitOfWork unitOfWork)
            {
                _mapper = mapper;
                _sysLoginRepo = sysUserRepo;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<SysLogin_InsertLogin_Response>> Handle(SysLogin_InsertLogin_Command command, CancellationToken cancellationToken)
            {
                SysLogin_InsertLogin_Response response = new SysLogin_InsertLogin_Response();
                SysLogin login;

                if (command.Request == null)
                {
                    return Result<SysLogin_InsertLogin_Response>.Fail(SysLogin_InsertLogin_Response.StatusCodes.Invalid_Request, SysLogin_InsertLogin_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.Login == null)
                {
                    return Result<SysLogin_InsertLogin_Response>.Fail(SysLogin_InsertLogin_Response.StatusCodes.Invalid_Request, SysLogin_InsertLogin_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.Login.Id != 0)
                {
                    return Result<SysLogin_InsertLogin_Response>.Fail(SysLogin_InsertLogin_Response.StatusCodes.Invalid_Request, SysLogin_InsertLogin_Response.StatusTexts.Invalid_Request);
                }

                try
                {
                    _unitOfWork.Open();
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _sysLoginRepo.SetTransaction(transaction);

                            var loginPayload = command.Request!.Login;
                            login = _mapper.Map<SysLogin>(loginPayload);

                            login = await _sysLoginRepo.InsertAsync(login);

                            await _unitOfWork.SaveChangesAsync(cancellationToken);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();                            
                            return Result<SysLogin_InsertLogin_Response>.Fail(SysLogin_InsertLogin_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                        }
                        finally
                        {
                            _unitOfWork.Close();
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    return Result<SysLogin_InsertLogin_Response>.Fail(SysLogin_InsertLogin_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }

                if (login != null)
                {
                    response.Login = _mapper.Map<SysLogin_OutputPayload>(login);
                }

                return Result<SysLogin_InsertLogin_Response>.Success(response, SysLogin_InsertLogin_Response.StatusCodes.Success);
            }
        }
    }
}