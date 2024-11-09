using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Domain.Configurations;

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : BaseController<ConfigController>
    {
        public ConfigController(ILogger<ConfigController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
        {
        }

        [HttpGet("GetMainConfig", Name = "ConfigController.GetMainConfig")]
        public IActionResult GetMainConfig()
        {
            var mainConfig = _configuration.GetSection(nameof(MainConfig))?.Get<MainConfig>() ?? new MainConfig();

            return Ok(mainConfig);
        }
    }
}