using Application.Commands;
using Application.Services;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.DTOs;
using Domain.Entities.Customers;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private readonly IUploadService _uploadService;
        public AccountController(IMediator mediator, IAuthService authService, IUploadService uploadService)
        {
            _mediator = mediator;
            _authService = authService;
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
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand dto)
        {
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _authService.JwtGenratedToken(refreshToken);
            return Ok(result);
        }

        [HttpPost("signout")]
        public IActionResult SignOut(string refreshToken)
        {
            var result = _authService.RevokeRefreshToken(refreshToken);
            return Ok(result);
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _uploadService.UploadFileAsync(file);
            return Ok(result);
        }
    }
}
