using Application.Commands.Admin;
using Application.Queries;
using Application.Queries.Admin;
using Domain.DTOs;
using Domain.DTOs.AccountSettingsDtos;
using Domain.DTOs.Admin;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseApiController
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-all-users")]
        [ProducesResponseType(typeof(List<GetAllUsersResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerAddresses([FromQuery]GetAllUsersQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpGet("get-customer-profile")]
        [ProducesResponseType(typeof(GetUserProfileAdminResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserProfile([FromQuery] GetCustomerProfileQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-producer-profile")]
        [ProducesResponseType(typeof(GetUserProfileAdminResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducerProfile([FromQuery] GetProducerProfileQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpPost("change-user-status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeUserStatus([FromBody] ChangeUserStatusCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpGet("get-all-producers-offers")]
        [ProducesResponseType(typeof(List<CollaborationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerAddresses([FromQuery] GetAllProducerOffersQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpGet("get-readonly-chat")]
        [ProducesResponseType(typeof(ReadOnlyChatResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReadonlyChat([FromQuery] GetReadOnlyChatQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
    }
}
