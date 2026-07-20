using Application.Commands.AccountSettingsFeature;
using Application.Commands.OffersFeature;
using Application.Commands.OffersFeature.CustomOffer;
using Application.Handlers.OffersFeature.CustomOffers;
using Application.Queries.AccountSettings;
using Application.Queries.OffersFeature.CustomOffers;
using Domain.DTOs.OfferDtos;
using Domain.DTOs.Review_RateDtos;
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
        [HttpPost("add-producer-design-to-gallery")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddToGallery([FromQuery] AddProducerDesignCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-all-producers-Profiles")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProfiles([FromQuery]GetAllProfilesQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-all-gallery")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllGallery([FromQuery] GetAllProducerDesignsQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
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

        [HttpGet("get-producer-custom-offer-by-id")]
        [ProducesResponseType(typeof(ProducerOfferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProducerCustomOfferById([FromQuery]GetProducerCustomOfferByIdQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpPost("accept-offer")]
        [ProducesResponseType( StatusCodes.Status200OK)]
        [ProducesResponseType( StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AcceptOffer (AcceptCustomOfferCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet ("get-customers-requests")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomersRequests([FromQuery]GetCustomersRequestsQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-customer-requests")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerRequests([FromQuery] GetCustomerRequestsQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-custom-requst-by-id")]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRequestById([FromQuery] GetCustomRequestByIdQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpDelete("delete-customer-custom-offer")]
        [ProducesResponseType(typeof(ReviewAndRateResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteOffer([FromQuery] DeclineCustomerOfferCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
    }
}
