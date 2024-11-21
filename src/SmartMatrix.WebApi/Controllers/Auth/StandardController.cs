using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;
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
            try
            {
                var result = await _mediator.Send(new SysUser_PerformLogin_Command
                {
                    Request = request
                });

                if (result.Succeeded && result.Data != null && result.Data.User != null)
                {
                    var user = result.Data.User;
                    // suppose login is valid
                    var login = user.Logins.FirstOrDefault(x => x.LoginName == request.LoginName);
                    if (login == null)
                    {
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, "Login failed"));
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
                        return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, "Update refresh token failed"));
                    }

                    user.ClearSecrets();
                    result.Data.Token = token;
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, ex.Message));
            }            
        }

        [HttpPost("renew-token")]
        public async Task<IActionResult> RenewToken(SysLogin_RenewToken_Request request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(-999, "Invalid token"));
            }

            var getLoginResult = await _mediator.Send(new SysLogin_GetFirstByRefreshToken_Query
            {
                Request = new SysLogin_GetFirstByRefreshToken_Request
                {
                    PartitionKey = request.PartitionKey,
                    RefreshToken = request.RefreshToken
                }
            });

            if (!getLoginResult.Succeeded || getLoginResult.Data == null)
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(-999, "Invalid token"));
            }
            
            var login = getLoginResult.Data;
            if (string.IsNullOrEmpty(login.LoginName))
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(-999, "Invalid token"));
            }
            if (login.RefreshTokenExpires < DateTime.UtcNow)
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(-999, "Invalid token"));
            }
            if (login.Status == SysLogin.StatusOptions.Disabled)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, "Invalid token"));
            }
            if (login.Status == SysLogin.StatusOptions.Deleted)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, "Invalid token"));
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

            if (!getUserResult.Succeeded || getUserResult.Data == null)
            {
                return Ok(Result<SysLogin_RenewToken_Response>.Fail(-999, "Invalid user"));
            }
            var user = getUserResult.Data;
            if (user.Status == SysLogin.StatusOptions.Disabled)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, "Invalid user"));
            }
            if (user.Status == SysLogin.StatusOptions.Deleted || user.IsDeleted)
            {
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, "Invalid user"));
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
                return Ok(Result<SysUser_PerformLogin_Response>.Fail(-999, "Update refresh token failed"));
            }

            var mappedToken = _mapper.Map<SysLogin_RenewToken_Response>(token);

            return Ok(Result<SysLogin_RenewToken_Response>.Success(mappedToken));
        }

        [HttpPost("update-secrets")]
        public async Task<IActionResult> UpdateSecrets(SysLogin_UpdateSecrets_Request request)
        {
            var entity = await _mediator.Send(new SysLogin_UpdateSecrets_Command
            {
                Request = request
            });
            return Ok(entity);
        }

        [HttpGet("get-first-by-username")]
        public async Task<IActionResult> GetFirstByUserName([FromQuery] SysUser_GetFirstByUserName_Request request)
        {
            var entity = await _mediator.Send(new SysUser_GetFirstByUserName_Query
            {
                Request = request
            });
            return Ok(entity);
        }

        [HttpGet("get-first-by-login-name")]
        public async Task<IActionResult> GetFirstByLoginName([FromQuery] SysUser_GetFirstByLoginName_Request request)
        {
            var entity = await _mediator.Send(new SysUser_GetFirstByLoginName_Query
            {
                Request = request
            });
            return Ok(entity);
        }
    }
}