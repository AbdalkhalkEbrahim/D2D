using Application.Commands.OffersFeature.CustomOffer;
using Domain.DTOs.OfferDtos;
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

        [HttpPost("customer-request-on-producer-design")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CustomerRequestedOffer(CustomerRequestOfferOnProducerDesignCommad dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpPost("producer-custom-offer")]
        [ProducesResponseType(typeof(ProducerOfferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ProducerCustomOffer(ProducerCustomOfferCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
    }
}
