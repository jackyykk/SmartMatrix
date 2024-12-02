using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.WsFed;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.Core.BaseClasses.Web;
using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Core.Identities.Messages;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/sysuser")]
    //[Authorize(Policy = WebConstants.Authorizations.Policies.Standard_Api_Policy)]
    public class SysUserController : BaseController<SysUserController>
    {
        private readonly IAuthenticatedUserService _userSvc;

        public SysUserController(ILogger<SysUserController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper, IAuthenticatedUserService userSvc)
            : base(logger, configuration, mediator, mapper)
        {
            _userSvc = userSvc;
        }

        /// <summary>
        /// Extract My Info from Token
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-my-info")]
        public IActionResult GetTokenContent()
        {
            SysUser_GetTokenContent_Response response = new SysUser_GetTokenContent_Response();
            SysTokenContent_OutputPayload tokenContent;

            if (_userSvc == null || string.IsNullOrEmpty(_userSvc.LoginNameIdentifier) || string.IsNullOrEmpty(_userSvc.UserNameIdentifier))
            {                
                return Ok(Result<SysUser_GetTokenContent_Response>.Fail(SysUser_GetTokenContent_Response.StatusCodes.Invalid_Request, SysUser_GetTokenContent_Response.StatusTexts.Invalid_Request));
            }

            tokenContent = new SysTokenContent_OutputPayload
            {
                LoginProviderName = _userSvc?.LoginProviderName,
                LoginNameIdentifier = _userSvc?.LoginNameIdentifier,
                UserNameIdentifier = _userSvc?.UserNameIdentifier,
                Email = _userSvc?.Email,
                Name = _userSvc?.Name,
                GivenName = _userSvc?.GivenName,
                Surname = _userSvc?.Surname                
            };

            _userSvc?.Claims.Where(x => x.Key == ClaimTypes.Role)
                .ToList().ForEach(x => tokenContent.Roles.Add(x.Value));
            
            response.TokenContent = tokenContent;

            return Ok(Result<SysUser_GetTokenContent_Response>.Success(response));
        }

        [HttpGet("getfirst-by_type")]
        public async Task<IActionResult> GetFirstByType([FromQuery] SysUser_GetFirstByType_Request request)
        {
            var result = await _mediator.Send(new SysUser_GetFirstByType_Query
            {
                Request = request
            });

            return Ok(result);
        }

        [HttpGet("getfirst-by_username")]
        public async Task<IActionResult> GetFirstByUserName([FromQuery] SysUser_GetFirstByUserName_Request request)
        {
            var result = await _mediator.Send(new SysUser_GetFirstByUserName_Query
            {
                Request = request
            });

            return Ok(result);
        }

        [HttpGet("getfirst-by_login_name")]
        public async Task<IActionResult> GetFirstByLoginName([FromQuery] SysUser_GetFirstByLoginName_Request request)
        {
            var result = await _mediator.Send(new SysUser_GetFirstByLoginName_Query
            {
                Request = request
            });

            return Ok(result);
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(SysUser_CreateUser_Request request)
        {
            var result = await _mediator.Send(new SysUser_CreateUser_Command
            {
                Request = request
            });

            return Ok(result);
        }
    }
}