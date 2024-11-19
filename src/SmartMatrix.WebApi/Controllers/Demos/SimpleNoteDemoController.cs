using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;
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

        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetById([FromQuery] SimpleNote_GetById_Request request)
        {
            SimpleNote note = new SimpleNote();

            var entity = await _mediator.Send(new SimpleNote_GetById_Query{
                Request = request
            });
            return Ok(entity);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SimpleNote_Create_Request request)
        {
            var entity = await _mediator.Send(new SimpleNote_Create_Command{
                Request = request
            });
            return Ok(entity);
        }
    }
}
