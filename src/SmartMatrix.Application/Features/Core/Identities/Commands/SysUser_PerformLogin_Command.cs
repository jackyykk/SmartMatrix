using AutoMapper;
using MediatR;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Core.DataSecurity;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

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
                SysUser_PerformLogin_Response response = new SysUser_PerformLogin_Response();

                if (command.Request == null)
                {
                    return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Invalid_Request, SysUser_PerformLogin_Response.StatusTexts.Invalid_Request);
                }

                if (string.IsNullOrEmpty(command.Request!.PartitionKey)
                    || string.IsNullOrEmpty(command.Request!.LoginName)
                    || string.IsNullOrEmpty(command.Request!.Password))
                {
                    return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Invalid_Request, SysUser_PerformLogin_Response.StatusTexts.Invalid_Request);
                }

                try
                {
                    string partitionKey = command.Request!.PartitionKey;
                    string loginName = command.Request!.LoginName;
                    string password = command.Request!.Password;

                    var user = await _sysUserRepo.GetFirstByLoginNameAsync(partitionKey, loginName);
                    if (user == null)
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.User_NotFound, SysUser_PerformLogin_Response.StatusTexts.Login_Failed);
                    }

                    var login = user.Logins.FirstOrDefault(x => x.LoginName == loginName);
                    if (login == null)
                    {
                        // Suppose user should have the login
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Login_NotFound, SysUser_PerformLogin_Response.StatusTexts.Login_Failed);
                    }
                    if (login.Status == SysLogin.StatusOptions.Disabled)
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Login_Disabled, SysUser_PerformLogin_Response.StatusTexts.Login_Failed);
                    }
                    if (login.Status == SysLogin.StatusOptions.Deleted || login.IsDeleted)
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Login_Deleted, SysUser_PerformLogin_Response.StatusTexts.Login_Failed);
                    }
                    if (!MyHashTool.VerifyPasswordHash(password, login.PasswordSalt, login.PasswordHash))
                    {
                        return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Password_NotMatch, SysUser_PerformLogin_Response.StatusTexts.Login_Failed);
                    }

                    var userPayload = _mapper.Map<SysUserPayload>(user);
                    response.User = userPayload;
                    response.Token = new SysTokenPayload();
                    return Result<SysUser_PerformLogin_Response>.Success(response, SysUser_PerformLogin_Response.StatusCodes.Success);
                }
                catch (Exception ex)
                {
                    return Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, ex.Message);
                }
            }
        }
    }
}