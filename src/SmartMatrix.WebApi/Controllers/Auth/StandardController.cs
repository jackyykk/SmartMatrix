using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(SysUser_PerformLogin_Request request)
        {
            var entity = await _mediator.Send(new SysUser_PerformLogin_Command{
                Request = request
            });
            return Ok(entity);
        }

        [HttpPost("compute-secrets")]
        public async Task<IActionResult> ComputeSecrets(SysLogin_ComputeSecrets_Request request)
        {
            var entity = await _mediator.Send(new SysLogin_ComputeSecrets_Command{
                Request = request
            });
            return Ok(entity);
        }

        [HttpGet("get-first-by-username")]
        public async Task<IActionResult> GetFirstByUserName([FromQuery] SysUser_GetFirstByUserName_Request request)
        {
            var entity = await _mediator.Send(new SysUser_GetFirstByUserName_Query{
                Request = request
            });
            return Ok(entity);
        }

        [HttpGet("get-first-by-login-name")]
        public async Task<IActionResult> GetFirstByLoginName([FromQuery] SysUser_GetFirstByLoginName_Request request)
        {
            var entity = await _mediator.Send(new SysUser_GetFirstByLoginName_Query{
                Request = request
            });
            return Ok(entity);
        }        
    }
}