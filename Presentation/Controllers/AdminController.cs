using Application.Commands.Admin;
using Application.Queries.Admin;
using Domain.DTOs.Admin;
using MediatR;
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
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQuery dto)
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
        public async Task<IActionResult> GetAllProducersOffers([FromQuery] GetAllProducerOffersQuery dto)
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
        [HttpGet("get-users-count")]
        [ProducesResponseType(typeof(UserCountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersCount()
        {
            var result = await _mediator.Send(new GetUsersCountQuery());
            return HandleResult(result);
        }
/*        [HttpGet("get-active-collaboration-count")]
        [ProducesResponseType(typeof(ActiveCollaborationCountResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetActiveCollaborationCount()
        {
            var result = await _mediator.Send(new CollaborationCountQuery());
            return HandleResult(result);
        }*/
        [HttpGet("get-recent-users")]
        [ProducesResponseType(typeof(List<RecentUsersResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentUsers()
        {
            var result = await _mediator.Send(new RecentUsersQuery());
            return HandleResult(result);
        }

        [HttpGet("get-ticket-by-id")]
        [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTicketById([FromQuery] GetTicketByIdQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpGet("get-all-tickets")]
        [ProducesResponseType(typeof(List<TicketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTickets([FromQuery] GetAllTicketsQuery dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        [HttpGet("get-recent-tickets")]
        [ProducesResponseType(typeof(List<TicketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentTickets()
        {
            var result = await _mediator.Send(new GetRecentTicketsQuery());
            return HandleResult(result);

        }
        [HttpPost("change-ticket-status")]
        [ProducesResponseType(typeof(List<TicketResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeTicketStatus([FromQuery] ChangeTicketStatusCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

    }
}
