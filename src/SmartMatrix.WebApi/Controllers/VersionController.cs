using System.Net.Mail;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VersionController : BaseController<VersionController>
    {        
        public VersionController(ILogger<VersionController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        [HttpGet("get-version", Name = "VersionController.GetVersion")]
        public IActionResult GetVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            // trim the commit hash following the first '+' character
            if (version != null)
            {
                var plusIndex = version.IndexOf('+');
                if (plusIndex > 0)
                {
                    version = version.Substring(0, plusIndex);
                }
            }

            return Ok(new { version });
        }
    }
}