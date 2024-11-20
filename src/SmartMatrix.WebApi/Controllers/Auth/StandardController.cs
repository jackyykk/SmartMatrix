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

                var token = Auth_Standard_Generate_TokenContent(LOGIN_PROVIDER_NAME, request.LoginName, user);
                
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

        [HttpPost("try-renew-refresh-token")]
        public async Task<IActionResult> TryRenewRefreshToken(SysLogin_TryRenewRefreshToken_Request request)
        {            
            JwtSecret secret = Auth_Get_JwtSecret();
            JwtContent jwtContent = TokenUtil.DecodeJwt(secret, request.AuthToken);
            
            if (jwtContent == null)
            {
                return Ok(Result<SysLogin_TryRenewRefreshToken_Response>.Fail(-999, "Invalid token"));
            }
            
            if (string.IsNullOrEmpty(jwtContent.LoginNameIdentifier) || string.IsNullOrEmpty(jwtContent.UserNameIdentifier))
            {
                return Ok(Result<SysLogin_TryRenewRefreshToken_Response>.Fail(-999, "Invalid token"));
            }

            var getUserResult = await _mediator.Send(new SysUser_GetFirstByUserName_Query
            {
                Request = new SysUser_GetFirstByUserName_Request
                {
                    PartitionKey = PartitionKeyOptions.SmartMatrix,
                    UserName = jwtContent.UserNameIdentifier                    
                }
            });

            if (!getUserResult.Succeeded || getUserResult.Data == null)
            {
                return Ok(Result<SysLogin_TryRenewRefreshToken_Response>.Fail(-999, "User not found"));
            }
            
            var user = getUserResult.Data;
            var login = user.Logins.FirstOrDefault(x => x.LoginName == jwtContent.LoginNameIdentifier);            
            
            if (login == null)
            {
                return Ok(Result<SysLogin_TryRenewRefreshToken_Response>.Fail(-999, "Login not found"));
            }

            if (login.RefreshToken != request.RefreshToken)
            {
                return Ok(Result<SysLogin_TryRenewRefreshToken_Response>.Fail(-999, "Invalid refresh token"));
            }

            if (login.RefreshTokenExpires < DateTime.UtcNow)
            {
                return Ok(Result<SysLogin_TryRenewRefreshToken_Response>.Fail(-999, "Invalid refresh token"));
            }

            var token = Auth_Standard_Generate_TokenContent(LOGIN_PROVIDER_NAME, jwtContent.LoginNameIdentifier, user);

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

            var mappedToken = _mapper.Map<SysLogin_TryRenewRefreshToken_Response>(token);

            return Ok(Result<SysLogin_TryRenewRefreshToken_Response>.Success(mappedToken));
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