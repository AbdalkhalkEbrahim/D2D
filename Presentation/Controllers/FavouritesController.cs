using Application.Commands.OffersFeature.CustomOffer;
using Application.Queries.OffersFeature.CustomOffers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : BaseApiController
    {
        private readonly IMediator _mediator;
        public FavouritesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add-to-favourites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(AddToFavouriteCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpDelete("remove-from-favourites")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFavourite(RemoveFromFavouritesCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result,204);
        }

        [HttpGet("favourites")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Favourites([FromQuery]GetFavouritesQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
    }
}
