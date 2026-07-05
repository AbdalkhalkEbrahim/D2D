using Application.Commands.OffersFeature;
using Application.Queries.OffersFeature;
using Domain.DTOs.OfferDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : BaseApiController
    {
        private readonly IMediator _mediator;

        public OfferController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("customer-publish-design")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        public async Task<IActionResult> CustomerPublishOffer(CustomerPublishOfferCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        //GET: PublishedOfferDetails
        [HttpPost("producer-customer-offer")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> ProducerCustomerOffer(ProducerCustomerOfferCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// for customer to get published design details by id
        /// </summary>

        [HttpGet("get-published-design-details")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPublishedDesignDetails([FromQuery] GetPublishedDesignByIdQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// for producer to get all published designs by customers
        /// </summary>

        [HttpGet("get-all-published-designs")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPublishedDesigns([FromQuery] GetAllPublishedesignsQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// for customer to edit published design details by id
        /// </summary>
        [HttpPatch("edit-published-design")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditPublishedDesign([FromBody] EditPublishedDesignCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpPatch("edit-producer-customer-offer")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditProducerCustomerOffer([FromBody] EditProducerCustomerOfferCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpGet("get-producer-offer-details")]
        [ProducesResponseType(typeof(ProducerOfferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducerOfferDetails([FromQuery] GetProducerCustomerOfferByIdQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpGet("get-Published-design-offers")]
        [ProducesResponseType(typeof(List<ProducerOfferResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProducerOffers([FromQuery] GetOffersOnPublishedDesignQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpPost("decline-producer-offer")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeclineProducerOffer([FromBody] DeclineProducerOfferCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

    }
}
