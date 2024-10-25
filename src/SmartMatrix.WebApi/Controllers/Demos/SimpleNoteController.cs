using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Demos.SimpleNotes.Queries;
using SmartMatrix.Domain.Entities.Demos;
using SmartMatrix.Domain.Messages.Demos.SimpleNotes;

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
