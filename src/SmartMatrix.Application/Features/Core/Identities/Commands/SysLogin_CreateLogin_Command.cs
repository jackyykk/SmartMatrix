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
    public class SysLogin_CreateLogin_Command : IRequest<Result<SysLogin_CreateLogin_Response>>
    {
        public SysLogin_CreateLogin_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysLogin_CreateLogin_Command, Result<SysLogin_CreateLogin_Response>>
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

            public async Task<Result<SysLogin_CreateLogin_Response>> Handle(SysLogin_CreateLogin_Command command, CancellationToken cancellationToken)
            {
                SysLogin_CreateLogin_Response response = new SysLogin_CreateLogin_Response();
                SysLogin login;

                if (command.Request == null)
                {
                    return Result<SysLogin_CreateLogin_Response>.Fail(SysLogin_CreateLogin_Response.StatusCodes.Invalid_Request, SysLogin_CreateLogin_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.Login == null)
                {
                    return Result<SysLogin_CreateLogin_Response>.Fail(SysLogin_CreateLogin_Response.StatusCodes.Invalid_Request, SysLogin_CreateLogin_Response.StatusTexts.Invalid_Request);
                }

                if (command.Request!.Login.Id != 0)
                {
                    return Result<SysLogin_CreateLogin_Response>.Fail(SysLogin_CreateLogin_Response.StatusCodes.Invalid_Request, SysLogin_CreateLogin_Response.StatusTexts.Invalid_Request);
                }

                try
                {
                    _unitOfWork.Open();
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _sysLoginRepo.SetTransaction(transaction);

                            var inputLogin = command.Request!.Login;
                            login = _mapper.Map<SysLogin>(inputLogin);

                            login = await _sysLoginRepo.InsertAsync(login);

                            await _unitOfWork.SaveChangesAsync(cancellationToken);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();                            
                            return Result<SysLogin_CreateLogin_Response>.Fail(SysLogin_CreateLogin_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                        }
                        finally
                        {
                            _unitOfWork.Close();
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    return Result<SysLogin_CreateLogin_Response>.Fail(SysLogin_CreateLogin_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }

                if (login != null)
                {
                    var outputLogin = _mapper.Map<SysLogin_OutputPayload>(login); 
                    response.Login = outputLogin;
                }

                return Result<SysLogin_CreateLogin_Response>.Success(response, SysLogin_CreateLogin_Response.StatusCodes.Success);
            }
        }
    }
}