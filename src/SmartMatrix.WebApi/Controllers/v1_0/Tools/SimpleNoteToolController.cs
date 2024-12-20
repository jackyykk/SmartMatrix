using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SmartMatrix.Application.Features.Tools.SimpleNoteTool.Commands;
using SmartMatrix.Application.Features.Tools.SimpleNoteTool.Queries;
using SmartMatrix.Domain.Tools.SimpleNoteTool.Messages;

namespace SmartMatrix.WebApi.Controllers.v1_0.Tools
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tools/simple_note_tool")]
    [ApiVersion("1.0")]
    public class SimpleNoteToolController : BaseController<SimpleNoteToolController>
    {
        public SimpleNoteToolController(ILogger<SimpleNoteToolController> logger, IConfiguration configuration, IMediator mediator, IMapper mapper)
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
