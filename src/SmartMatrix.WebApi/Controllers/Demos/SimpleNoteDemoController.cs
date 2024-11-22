using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Demos.SimpleNoteDemo.Queries;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages;

namespace SmartMatrix.WebApi.Controllers.Demos
{
    [ApiController]
    [Route("api/demos/simple_note_demo")]    
    public class SimpleNoteDemoController : BaseController<SimpleNoteDemoController>
    {
        public SimpleNoteDemoController(ILogger<SimpleNoteDemoController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
            : base(logger, configuration, mediator, mapper)
        {
        }

        [HttpGet("get-by_id")]
        public async Task<IActionResult> GetById([FromQuery] SimpleNote_GetById_Request request)
        {
            var result = await _mediator.Send(new SimpleNote_GetById_Query{
                Request = request
            });
            return Ok(result);
        }

        [HttpGet("getlist-by_owner")]
        public async Task<IActionResult> GetListByOwner([FromQuery] SimpleNote_GetListByOwner_Request request)
        {            
            var result = await _mediator.Send(new SimpleNote_GetListByOwner_Query{
                Request = request
            });
            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] SimpleNote_Create_Request request)
        {
            var result = await _mediator.Send(new SimpleNote_Create_Command{
                Request = request
            });
            return Ok(result);
        }
    }
}
