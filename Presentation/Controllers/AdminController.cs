using Application.Queries;
using Application.Queries.AccountSettings;
using Domain.DTOs;
using Domain.DTOs.AccountSettingsDtos;
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


    }
}
