using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.WebApi.Controllers.Core
{
    [ApiController]
    [Route("api/core/syslogin")]
    public class SysLoginController : BaseController<SysLoginController>
    {
        public SysLoginController(ILogger<SysLoginController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        [HttpPost("update-secrets")]
        public async Task<IActionResult> UpdateSecrets(SysLogin_UpdateSecrets_Request request)
        {
            var result = await _mediator.Send(new SysLogin_UpdateSecrets_Command
            {
                Request = request
            });

            return Ok(result);
        }

        [HttpPost("insert-login")]
        public async Task<IActionResult> InsertLogin(SysLogin_InsertLogin_Request request)
        {
            var result = await _mediator.Send(new SysLogin_InsertLogin_Command
            {
                Request = request
            });

            return Ok(result);
        }       
    }
}