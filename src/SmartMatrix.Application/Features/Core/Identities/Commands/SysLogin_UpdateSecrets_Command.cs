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
        public SysLogin_UpdateSecrets_Request? Request { get; set; }

        public class Handler : BaseHandler, IRequestHandler<SysLogin_UpdateSecrets_Command, Result<SysLogin_UpdateSecrets_Response>>
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

            public async Task<Result<SysLogin_UpdateSecrets_Response>> Handle(SysLogin_UpdateSecrets_Command command, CancellationToken cancellationToken)
            {
                SysLogin_UpdateSecrets_Response response = new SysLogin_UpdateSecrets_Response();

                if (command.Request == null)
                {
                    return Result<SysLogin_UpdateSecrets_Response>.Fail(SysLogin_UpdateSecrets_Response.StatusCodes.Invalid_Request, SysLogin_UpdateSecrets_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(command.Request!.PartitionKey))                        
                {
                    return Result<SysLogin_UpdateSecrets_Response>.Fail(SysLogin_UpdateSecrets_Response.StatusCodes.Invalid_Request, SysLogin_UpdateSecrets_Response.StatusTexts.Invalid_Request);
                }

                try
                {
                    _unitOfWork.Open();
                    using (var transaction = _unitOfWork.BeginTransaction())
                    {
                        try
                        {
                            _sysLoginRepo.SetTransaction(transaction);

                            string partitionKey = command.Request!.PartitionKey;                    

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

                                    login.PasswordHash = StandardHashTool.ComputePasswordHash(password, out string salt);
                                    login.PasswordSalt = salt;
                                    
                                    await _sysLoginRepo.UpdateSecretAsync(login.Id, login.Password, login.PasswordHash, login.PasswordSalt);
                                    response.UpdatedIds.Add(login.Id);                                
                                }
                            }
                                                                                    
                            await _unitOfWork.SaveChangesAsync(cancellationToken);
                            transaction.Commit();                        
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            return Result<SysLogin_UpdateSecrets_Response>.Fail(SysLogin_UpdateSecrets_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                        }                    
                        finally
                        {
                            _unitOfWork.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    return Result<SysLogin_UpdateSecrets_Response>.Fail(SysLogin_UpdateSecrets_Response.StatusCodes.Unknown_Error, GetErrorMessage(ex));
                }

                return Result<SysLogin_UpdateSecrets_Response>.Success(response, SysLogin_UpdateSecrets_Response.StatusCodes.Success);                        
            }
        }
    }
}