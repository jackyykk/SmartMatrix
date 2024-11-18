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

        [HttpGet("get-first-by-username", Name = "StandardController.GetFirstByUserName")]
        public async Task<IActionResult> GetFirstByUserName([FromQuery] GetFirstSysUserByUserNameRequest request)
        {
            var entity = await _mediator.Send(new GetFirstSysUserByUserNameQuery{
                Request = request
            });
            return Ok(entity);
        }
    }
}