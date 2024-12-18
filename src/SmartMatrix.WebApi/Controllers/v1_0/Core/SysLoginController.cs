using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Core.Identities.Commands;
using SmartMatrix.Domain.Core.Identities.Messages;

namespace SmartMatrix.WebApi.Controllers.v1_0.Core
{
    [ApiController]
    [Route("api/v{version:apiVersion}/core/syslogin")]
    [ApiVersion("1.0")]
    //[Authorize(Policy = WebConstants.Authorizations.Policies.Standard_Api_Policy)]
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

        [HttpPost("create-login")]
        public async Task<IActionResult> CreateLogin(SysLogin_CreateLogin_Request request)
        {
            var result = await _mediator.Send(new SysLogin_CreateLogin_Command
            {
                Request = request
            });

            return Ok(result);
        }       
    }
}