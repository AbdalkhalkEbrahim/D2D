using Application.Commands.DesignFeature;
using Application.Commands.OffersFeature;
using Application.Queries.DesignFeature;
using Azure;
using Domain.DTOs.DesignDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        public async Task<IActionResult> SvaeDesign([FromForm] SaveDesignCommand dto)
        {
            var result = await _mediator.Send(dto);
           /* if (result.IsSuccess)
                return CreatedAtRoute("get-design", result.Value, result);*/
            return HandleResult(result);
        }

        [HttpDelete("delete-design/{Id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDesign(Guid Id)
        {
            var result = await _mediator.Send(new DeleteDesignCommand { Id = Id });
            return HandleResult(result, 204);
        }
        [HttpGet("get-customer-designs")]
        [ProducesResponseType(typeof(List<DesignResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerDesigns([FromQuery] GetAllDesignsQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpPost("published-to-drafted")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PublishedToDrafted([FromBody] PublishedToDraftedCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }       
    }
}
