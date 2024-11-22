using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Queries;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/sysuser")]
    public class SysUserController : BaseController<SysUserController>
    {
        public SysUserController(ILogger<SysUserController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
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
    }
}