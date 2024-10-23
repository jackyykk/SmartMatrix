using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmartMatrix.Application.Configurations;

namespace SmartMatrix.WebApi.Controllers
{
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        protected readonly IConfiguration _configuration;

        protected BaseController(ILogger<T> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }        
    }
}