using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Domain.Configurations;

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : BaseController<ConfigController>
    {
        public ConfigController(ILogger<ConfigController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        [HttpGet("get-main_config", Name = "ConfigController.GetMainConfig")]
        public IActionResult GetMainConfig()
        {
            var mainConfig = _configuration.GetSection(nameof(MainConfig))?.Get<MainConfig>() ?? new MainConfig();

            return Ok(mainConfig);
        }
    }
}