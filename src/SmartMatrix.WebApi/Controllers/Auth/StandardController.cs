using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;
using SmartMatrix.WebApi.Utils;
using static SmartMatrix.Domain.Core.Identities.DbEntities.SysUser;

namespace SmartMatrix.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/standard")]
    public class StandardController : BaseController<StandardController>
    {
        const string LOGIN_PROVIDER_NAME = "Standard";

        public StandardController(ILogger<StandardController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(SysUser_PerformLogin_Request request)
        {
            if (request == null)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Invalid_Request, "Invalid request"));
            }

            if (string.IsNullOrEmpty(request!.PartitionKey)
                || string.IsNullOrEmpty(request!.LoginName)
                || string.IsNullOrEmpty(request!.Password))
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Invalid_Request, "Invalid request"));
            }

            try
            {
                var result = await _mediator.Send(new SysUser_PerformLogin_Command
                {
                    Request = request
                });

                if (result.Succeeded && result.Data != null && result.Data.User != null)
                {
                    var user = result.Data.User;
                    // Suppose login is valid
                    var login = user.Logins.FirstOrDefault(x => x.LoginName == request.LoginName);
                    if (login == null)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Login_NotFound, "Login failed"));
                    }                 

                    var token = Auth_Standard_Generate_SysToken(LOGIN_PROVIDER_NAME, request.LoginName, user);
                    
                    login.RefreshToken = token.RefreshToken;                
                    login.RefreshTokenExpires = token.RefreshToken_Expires;

                    var updateRefreshTokenResult = await _mediator.Send(new SysLogin_UpdateRefreshToken_Command
                    {
                        Request = new SysLogin_UpdateRefreshToken_Request
                        {
                            Login = login
                        }
                    });
                    if (!updateRefreshTokenResult.Succeeded)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.RefreshToken_Update_Failed, "Login failed"));
                    }
                    
                    user.ClearSecrets();

                    var tokenPayload = _mapper.Map<SysTokenPayload>(token);
                    result.Data.Token = tokenPayload;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysUser_PerformLogin_Response.StatusCodes.Unknown_Error, ex.Message));
            }            
        }

        [HttpPost("renew-token")]
        public async Task<IActionResult> RenewToken(SysLogin_RenewToken_Request request)
        {
            SysLogin_RenewToken_Response response = new SysLogin_RenewToken_Response();

            if (request == null)
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Invalid_Request, SysLogin_RenewToken_Response.StatusTexts.Invalid_Request));
            }

            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Invalid_Request, SysLogin_RenewToken_Response.StatusTexts.Invalid_Request));
            }

            try
            {             
                var getLoginResult = await _mediator.Send(new SysLogin_GetFirstByRefreshToken_Query
                {
                    Request = new SysLogin_GetFirstByRefreshToken_Request
                    {
                        PartitionKey = request.PartitionKey,
                        RefreshToken = request.RefreshToken
                    }
                });

                if (!getLoginResult.Succeeded || getLoginResult.Data == null || getLoginResult.Data.Login == null)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_NotFound, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                
                var login = getLoginResult.Data.Login;
                if (string.IsNullOrEmpty(login.LoginName))
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.LoginName_Empty, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (login.RefreshTokenExpires < DateTime.UtcNow)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.RefreshToken_Expired, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (login.Status == SysLogin.StatusOptions.Disabled)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_Disabled, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (login.Status == SysLogin.StatusOptions.Deleted)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Login_Deleted, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                } 

                // Get user
                var getUserResult = await _mediator.Send(new SysUser_GetById_Query
                {
                    Request = new SysUser_GetById_Request
                    {
                        PartitionKey = request.PartitionKey,
                        Id = login.SysUserId
                    }
                });

                if (!getUserResult.Succeeded || getUserResult.Data == null || getUserResult.Data.User == null)
                {
                    return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_NotFound, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                var user = getUserResult.Data.User;
                if (user.Status == SysLogin.StatusOptions.Disabled)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_Disabled, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }
                if (user.Status == SysLogin.StatusOptions.Deleted || user.IsDeleted)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.User_Deleted, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }

                var token = Auth_Standard_Generate_SysToken(LOGIN_PROVIDER_NAME, login.LoginName, user);

                login.RefreshToken = token.RefreshToken;                
                login.RefreshTokenExpires = token.RefreshToken_Expires;

                var updateRefreshTokenResult = await _mediator.Send(new SysLogin_UpdateRefreshToken_Command
                {
                    Request = new SysLogin_UpdateRefreshToken_Request
                    {
                        Login = login
                    }
                });
                if (!updateRefreshTokenResult.Succeeded)
                {
                    return Ok(Result<SysUser_PerformLogin_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.RefreshToken_Update_Failed, SysLogin_RenewToken_Response.StatusTexts.Invalid_Token));
                }

                var tokenPayload = _mapper.Map<SysTokenPayload>(token);
                response.Token = tokenPayload;
                return Ok(Result<SysLogin_RenewToken_Response>.Success(response));
            }
            catch (Exception ex)
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(SysLogin_RenewToken_Response.StatusCodes.Unknown_Error, ex.Message));
            }            
        }            
    }
}