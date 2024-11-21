using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Core.DataSecurity;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Commands
{
    public class SysUser_PerformLogin_Command : IRequest<Result<SysUser_PerformLogin_Response>>
    {        
        public SysUser_PerformLogin_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysUser_PerformLogin_Command, Result<SysUser_PerformLogin_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<SysUser_PerformLogin_Response>> Handle(SysUser_PerformLogin_Command command, CancellationToken cancellationToken)
            {                
                try
                {
                    if (command.Request == null)
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.InvalidRequest, "Invalid Request");
                    }

                    if (string.IsNullOrEmpty(command.Request!.PartitionKey)
                        || string.IsNullOrEmpty(command.Request!.LoginName)
                        || string.IsNullOrEmpty(command.Request!.Password))
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.InvalidRequest, "Invalid Request");
                    }

                    string partitionKey = command.Request!.PartitionKey;
                    string loginName = command.Request!.LoginName;
                    string password = command.Request!.Password;

                    var user = await _sysUserRepo.GetFirstByLoginNameAsync(partitionKey, loginName);
                    if (user == null)
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.UserNotFound, "Login failed");
                    }

                    var login = user.Logins.FirstOrDefault(x => x.LoginName == loginName);
                    if (login == null)
                    {
                        // Suppose user should have the login
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.LoginNotFound, "Login failed");
                    }
                    if (login.Status == SysLogin.StatusOptions.Disabled)
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.LoginDisabled, "Login failed");
                    }
                    if (login.Status == SysLogin.StatusOptions.Deleted || login.IsDeleted)
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.LoginDeleted, "Login failed");
                    }                    
                    if (!MyHashTool.VerifyPasswordHash(password, login.PasswordSalt, login.PasswordHash))
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.InvalidPassword, "Login failed");
                    }
                    
                    var response = new SysUser_PerformLogin_Response
                    {
                        User = user,
                        Token = new SysToken()
                    };                            
                    return Result<SysUser_PerformLogin_Response>.Success(response);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.UnknownError, ex.Message);
                }                
            }
        }
    }
}