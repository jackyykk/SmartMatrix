using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/standard")]
    public class StandardController : BaseController<StandardController>
    {
        const string LOGIN_PROVIDER_NAME = "Standard";

        public StandardController(ILogger<StandardController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
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
                login.RefreshTokenExpires = token.RefreshTokenExpires;

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