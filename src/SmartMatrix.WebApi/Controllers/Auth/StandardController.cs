using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.WebApi.Controllers.Auth
{
    [ApiController]
    [Route("api/auth/standard")]
    public class StandardController : BaseController<StandardController>
    {
        public StandardController(ILogger<StandardController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
        {
        }

        [HttpGet("get-user-by-username", Name = "StandardController.GetUserByUserName")]
        public async Task<IActionResult> GetUserByUserName([FromQuery] SysUser_GetFirstByUserName_Request request)
        {
            var entity = await _mediator.Send(new SysUser_GetFirstByUserName_Query{
                Request = request
            });
            return Ok(entity);
        }

        [HttpGet("get-user-by-login-name", Name = "StandardController.GetUserByLoginName")]
        public async Task<IActionResult> GetUserByLoginName([FromQuery] SysUser_GetFirstByLoginName_Request request)
        {
            var entity = await _mediator.Send(new SysUser_GetFirstByLoginName_Query{
                Request = request
            });
            return Ok(entity);
        }
    }
}