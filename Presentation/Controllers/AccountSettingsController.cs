using Application.Commands.AccountSettingsFeature;
using Application.Commands.SettingsFeature;
using Application.Queries.AccountSettings;
using Domain.DTOs.AccountSettingsDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountSettingsController : BaseApiController
    {
        private readonly IMediator _mediator;
        public AccountSettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Changes user password.
        /// </summary>
        /// <remarks>
        /// Requires valid reset token or authenticated user.
        /// </remarks>

        [HttpPost("change-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpPost("add-new-address")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddNewAddress(AddNewAddressCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }


        [HttpGet("get-address/{id:int}", Name = "get-address")]
        [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var result = await _mediator.Send(new GetAddressByIdQuery { AddressId = id });
            return HandleResult(result);
        }

        [HttpPatch("edit-address")]
        [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditAddress(EditAddressCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-customer-addresses/{customerId:guid}")]
        [ProducesResponseType(typeof(List<AddressResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerAddresses(string customerId)
        {
            var result = await _mediator.Send(new GetCustomerAdrressesQuery { CustomerId = customerId });
            return HandleResult(result);
        }
        [HttpDelete("delete-address/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var result = await _mediator.Send(new DeleteAddressCommand { Id = id });
            return HandleResult(result, 204);
        }


        [HttpGet("get-customer-profile/{customerId:guid}")]
        [ProducesResponseType(typeof(CustomerProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomerProfile(string customerId)
        {
            var result = await _mediator.Send(new GetCustomerProfileQuery { CustomerId = customerId });
            return HandleResult(result);
        }


        [HttpGet("get-profile/{userId:guid}")]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile(string userId)
        {
            var result = await _mediator.Send(new GetProfileQuery { UserId = userId });
            return HandleResult(result);
        }
        [HttpPatch("edit-profile")]
        [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpPost("change-email")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpPost("add-ticket")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddTicket(AddTicketCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpPost("add-balance")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddBalance(AddBalanceCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-balance")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBalance([FromQuery]GetUserBalanceQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpPost("add-to-gallery")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddToGallery([FromQuery] PushToProducerGalleryCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpGet("get-all-gallery")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllGallery([FromQuery] GetAllGalleryQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
    }
}
