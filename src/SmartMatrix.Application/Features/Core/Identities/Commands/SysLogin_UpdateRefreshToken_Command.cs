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
    public class SysLogin_UpdateRefreshToken_Command : IRequest<Result<SysLogin_UpdateRefreshToken_Response>>
    {        
        public SysLogin_UpdateRefreshToken_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysLogin_UpdateRefreshToken_Command, Result<SysLogin_UpdateRefreshToken_Response>>
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

            public async Task<Result<SysLogin_UpdateRefreshToken_Response>> Handle(SysLogin_UpdateRefreshToken_Command query, CancellationToken cancellationToken)
            {
                SysLogin_UpdateRefreshToken_Response response = new SysLogin_UpdateRefreshToken_Response();

                if (query.Request == null)
                {
                    return Result<SysLogin_UpdateRefreshToken_Response>.Fail(SysLogin_UpdateRefreshToken_Response.StatusCodes.Invalid_Request, SysLogin_UpdateRefreshToken_Response.StatusTexts.Invalid_Request);
                }

                if (query.Request!.Login == null)                        
                {
                    return Result<SysLogin_UpdateRefreshToken_Response>.Fail(SysLogin_UpdateRefreshToken_Response.StatusCodes.Invalid_Request, SysLogin_UpdateRefreshToken_Response.StatusTexts.Invalid_Request);
                }

                _unitOfWork.Open();
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        _sysLoginRepo.SetTransaction(transaction);

                        var login = query.Request!.Login;                    

                        await _sysLoginRepo.UpdateRefreshTokenAsync(login);
                                                                                                        
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        transaction.Commit();                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Result<SysLogin_UpdateRefreshToken_Response>.Fail(SysLogin_UpdateRefreshToken_Response.StatusCodes.Unknown_Error, ex.Message);
                    }                    
                    finally
                    {
                        _unitOfWork.Close();
                    }
                }

                return Result<SysLogin_UpdateRefreshToken_Response>.Success(response, SysLogin_UpdateRefreshToken_Response.StatusCodes.Success);                        
            }
        }
    }
}