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
    public class SysLogin_UpdateSecrets_Command : IRequest<Result<SysLogin_UpdateSecrets_Response>>
    {
        public static class StatusCodes
        {
            public const int Success = 0;            
            public const int UnknownError = -1;
            public const int InvalidRequest = -101;
        }

        public SysLogin_UpdateSecrets_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysLogin_UpdateSecrets_Command, Result<SysLogin_UpdateSecrets_Response>>
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

            public async Task<Result<SysLogin_UpdateSecrets_Response>> Handle(SysLogin_UpdateSecrets_Command query, CancellationToken cancellationToken)
            {
                SysLogin_UpdateSecrets_Response response = new SysLogin_UpdateSecrets_Response();

                if (query.Request == null)
                {
                    return Result<SysLogin_UpdateSecrets_Response>.Fail(StatusCodes.InvalidRequest, "Request is null");
                }

                if (string.IsNullOrEmpty(query.Request!.PartitionKey))                        
                {
                    return Result<SysLogin_UpdateSecrets_Response>.Fail(StatusCodes.InvalidRequest, "Invalid Request");
                }

                _unitOfWork.Open();
                using (var transaction = _unitOfWork.BeginTransaction())
                {
                    try
                    {
                        _sysLoginRepo.SetTransaction(transaction);

                        string partitionKey = query.Request!.PartitionKey;                    

                        var logins = await _sysLoginRepo.GetListAsync(partitionKey);
                        if (logins != null && logins.Count > 0)
                        {
                            foreach (var login in logins)
                            {                                
                                if (!string.IsNullOrEmpty(login.PasswordHash) || !string.IsNullOrEmpty(login.PasswordSalt))
                                {
                                    continue;
                                }

                                if (string.IsNullOrEmpty(login.Password))
                                {
                                    continue;
                                }

                                // Only compute the secrets for the logins that have password and no password hash and salt
                                string password = login.Password;

                                login.PasswordHash = MyHashTool.ComputePasswordHash(password, out string salt);
                                login.PasswordSalt = salt;
                                
                                await _sysLoginRepo.UpdateSecretAsync(login);
                                response.UpdatedIds.Add(login.Id);                                
                            }
                        }
                                                                                
                        await _unitOfWork.SaveChangesAsync(cancellationToken);
                        transaction.Commit();                        
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return Result<SysLogin_UpdateSecrets_Response>.Fail(-1, ex.Message);
                    }                    
                    finally
                    {
                        _unitOfWork.Close();
                    }
                }

                return Result<SysLogin_UpdateSecrets_Response>.Success(response);                        
            }
        }
    }
}