using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Application.Interfaces.DataAccess.Transactions;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Core.DataSecurity;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Commands
{
    public class SysLogin_UpdateTokens_Command : IRequest<Result<SysLogin_UpdateTokens_Response>>
    {
        public SysLogin_UpdateTokens_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysLogin_UpdateTokens_Command, Result<SysLogin_UpdateTokens_Response>>
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

            public async Task<Result<SysLogin_UpdateTokens_Response>> Handle(SysLogin_UpdateTokens_Command command, CancellationToken cancellationToken)
            {
                SysLogin_UpdateTokens_Response response = new SysLogin_UpdateTokens_Response();

                if (command.Request == null)
                {
                    return Result<SysLogin_UpdateTokens_Response>.Fail(SysLogin_UpdateTokens_Response.StatusCodes.Invalid_Request, SysLogin_UpdateTokens_Response.StatusTexts.Invalid_Request);
                }

                // one-time token is optional
                if (command.Request!.LoginId <= 0
                    || string.IsNullOrEmpty(command.Request!.RefreshToken)
                    || command.Request!.RefreshTokenExpires == null)
                {
                    return Result<SysLogin_UpdateTokens_Response>.Fail(SysLogin_UpdateTokens_Response.StatusCodes.Invalid_Request, SysLogin_UpdateTokens_Response.StatusTexts.Invalid_Request);
                }

                try
                {
                    _unitOfWork.Open();
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _sysLoginRepo.SetTransaction(transaction);

                            int loginId = command.Request!.LoginId;
                            string refreshToken = command.Request!.RefreshToken;
                            DateTime refreshTokenExpires = command.Request!.RefreshTokenExpires.Value;
                            string? oneTimeToken = command.Request!.OneTimeToken;
                            DateTime? oneTimeTokenExpires = command.Request!.OneTimeTokenExpires.HasValue ? command.Request!.OneTimeTokenExpires.Value : null;

                            await _sysLoginRepo.UpdateTokensAsync(loginId, refreshToken, refreshTokenExpires, oneTimeToken, oneTimeTokenExpires);

                            await _unitOfWork.SaveChangesAsync(cancellationToken);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();                            
                            return Result<SysLogin_UpdateTokens_Response>.Fail(SysLogin_UpdateTokens_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                        }
                        finally
                        {
                            _unitOfWork.Close();
                        }
                    }
                }
                catch (Exception ex)
                {                    
                    return Result<SysLogin_UpdateTokens_Response>.Fail(SysLogin_UpdateTokens_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }

                return Result<SysLogin_UpdateTokens_Response>.Success(response, SysLogin_UpdateTokens_Response.StatusCodes.Success);
            }
        }
    }
}