using Application.Commands.OffersFeature;
using Application.Commands.OffersFeature.CustomOffer;
using Application.Handlers.OffersFeature.CustomOffers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomOffersController : BaseApiController
    {
        private readonly IMediator _mediator;
        public CustomOffersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("edit-producer-published-design-from-gallery")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditPublishedDesign([FromBody] EditProducerPublishedDesignFromGalleryCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpPost("delete-design-from-gallery")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeletePublishedDesign([FromBody] SoftDeleteDesignFromGalleryCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
    }
}
