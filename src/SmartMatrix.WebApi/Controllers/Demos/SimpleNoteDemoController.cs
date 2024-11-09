using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Entities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.WebApi.Controllers.Demos
{
    [ApiController]
    [Route("api/demos/simple-note-demo")]
    public class SimpleNoteDemoController : BaseController<SimpleNoteDemoController>
    {
        public SimpleNoteDemoController(ILogger<SimpleNoteDemoController> logger, IConfiguration configuration, IMediator mediator)
            : base(logger, configuration, mediator)
        {
        }

        [HttpGet("get-by-id", Name = "SimpleNoteDemoController.GetById")]
        public async Task<IActionResult> GetById([FromQuery] GetSimpleNoteByIdRequest request)
        {
            SimpleNote note = new SimpleNote();

            var entity = await _mediator.Send(new GetSimpleNoteByIdQuery{
                Request = request
            });
            return Ok(entity);
        }

        [HttpPost("create", Name = "SimpleNoteDemoController.Create")]
        public async Task<IActionResult> Create([FromBody] CreateSimpleNoteRequest request)
        {
            var entity = await _mediator.Send(new CreateSimpleNoteCommand{
                Request = request
            });
            return Ok(entity);
        }
    }
}
