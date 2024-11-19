using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Core.DataSecurity;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.Application.Features.Core.Identities.Commands
{
    public class SysUser_Login_Command : IRequest<Result<SysUser_Login_Response>>
    {
        public static class StatusCodes
        {
            public const int Success = 0;            
            public const int UnknownError = -1;
            public const int InvalidRequest = -101;
            public const int UserNotFound = -201;
            public const int LoginNotFound = -202;
            public const int LoginDisabled = -203;
            public const int LoginDeleted = -204;
            public const int InvalidPassword = -301;
        }

        public SysUser_Login_Request? Request { get; set; }

        public class Handler : IRequestHandler<SysUser_Login_Command, Result<SysUser_Login_Response>>
        {
            private readonly IMapper _mapper;
            private readonly ISysUserRepo _sysUserRepo;
            
            public Handler(IMapper mapper, ISysUserRepo sysUserRepo)
            {                
                _mapper = mapper;
                _sysUserRepo = sysUserRepo;
            }

            public async Task<Result<SysUser_Login_Response>> Handle(SysUser_Login_Command query, CancellationToken cancellationToken)
            {                
                try
                {
                    if (query.Request == null)
                    {
                        return Result<SysUser_Login_Response>.Fail(StatusCodes.InvalidRequest, "Request is null");
                    }

                    if (string.IsNullOrEmpty(query.Request!.PartitionKey)
                        || string.IsNullOrEmpty(query.Request!.LoginName)
                        || string.IsNullOrEmpty(query.Request!.Password))
                    {
                        return Result<SysUser_Login_Response>.Fail(StatusCodes.InvalidRequest, "Invalid Request");
                    }

                    string partitionKey = query.Request!.PartitionKey;
                    string loginName = query.Request!.LoginName;
                    string password = query.Request!.Password;

                    var user = await _sysUserRepo.GetFirstByLoginNameAsync(partitionKey, loginName);
                    if (user == null)
                    {
                        return Result<SysUser_Login_Response>.Fail(StatusCodes.UserNotFound, "Login failed");
                    }

                    var login = user.Logins.FirstOrDefault(x => x.LoginName == loginName);
                    if (login == null)
                    {
                        // Suppose user should have the login
                        return Result<SysUser_Login_Response>.Fail(StatusCodes.LoginNotFound, "Login failed");
                    }
                    if (login.Status == SysLogin.StatusOptions.Disabled)
                    {
                        return Result<SysUser_Login_Response>.Fail(StatusCodes.LoginDisabled, "Login failed");
                    }
                    if (login.Status == SysLogin.StatusOptions.Deleted || user.IsDeleted)
                    {
                        return Result<SysUser_Login_Response>.Fail(StatusCodes.LoginDeleted, "Login failed");
                    }

                    if (!MyHashTool.Verify(loginName, password, login.PasswordHash))
                    {
                        return Result<SysUser_Login_Response>.Fail(StatusCodes.InvalidPassword, "Login failed");
                    }

                    var mappedUser = _mapper.Map<SysUser_Login_Response>(user);
                    mappedUser.ClearLoginPasswords();                    
                    return Result<SysUser_Login_Response>.Success(mappedUser);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_Login_Response>.Fail(StatusCodes.UnknownError, ex.Message);
                }                
            }
        }
    }
}