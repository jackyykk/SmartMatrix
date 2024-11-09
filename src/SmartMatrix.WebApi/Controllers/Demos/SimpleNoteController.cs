using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Entities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.WebApi.Controllers.Demos
{
    [ApiController]
    [Route("api/demos/[controller]")]
    public class SimpleNoteController : BaseController<SimpleNoteController>
    {
        public SimpleNoteController(ILogger<SimpleNoteController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
        {
        }

        [HttpGet(Name = "GetById")]
        public async Task<IActionResult> GetById([FromQuery] GetSimpleNoteByIdRequest request)
        {
            SimpleNote note = new SimpleNote();

            var entity = await _mediator.Send(new GetSimpleNoteByIdQuery{
                Request = request
            });
            return Ok(entity);
        }

    }
}
