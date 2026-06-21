using Application.Commands.DesignFeature;
using Application.Queries.DesignFeature;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignController : BaseApiController
    {
        private readonly IMediator _mediator;

        public DesignController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-design/{Id:guid}", Name = "get-design")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDesign(Guid Id)
        {
            var result = await _mediator.Send(new GetDesignQuery { Id = Id });
            return HandleResult(result);
        }

        [HttpPost("save-design")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SvaeDesign([FromForm]SaveDesignCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleaAndCreatedAtActionResult(result, result.Value, "get-design");
            //return HandleResult(result);
        }

        [HttpDelete("delete-design/{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>DeleteDesign(Guid Id)
        {
            var result = await _mediator.Send(new DeleteDesignCommand { Id = Id });
            return HandleResult(result,204);
        }

    }
}
