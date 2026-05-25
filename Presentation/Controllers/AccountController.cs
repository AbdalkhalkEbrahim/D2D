using Application.Commands;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Domain.Entities.Customers;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Settings;
using Microsoft.Extensions.Options;
using Application.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IUploadService _uploadService;

        public AccountController(IMediator mediator, IUploadService uploadService)
        {
            _mediator = mediator;
            _uploadService = uploadService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(SendOtpCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [Authorize(Roles ="Producer")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            return Ok(await _uploadService.UploadFileAsync(file));
        }
        [HttpPost("customer-registeration")]
        public async Task<IActionResult> CustomerRegisteration(CustomerRegisterationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }
        [HttpPost("producer-registeration")]
        public async Task<IActionResult> ProducerRegisteration(ProducerRegisterrationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }
        [HttpPost("designer-registeration")]
        public async Task<IActionResult> DesignerRegisteration(DesignerRegesterationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);

        }
    }
}
