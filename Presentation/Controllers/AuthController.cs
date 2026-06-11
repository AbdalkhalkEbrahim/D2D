using Application.Commands;
using Application.Commands.RegisterationFeature;
using Application.Commands.SettingsFeature;
using Application.Interfaces;
using Domain.DTOs;
using Domain.DTOs.AuthDtos;
using Domain.DTOs.ModelDtos;
using Domain.DTOs.RegisterationDtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly IAuthService _authService;
        private readonly IUploadService _uploadService;
        private readonly IIdentityValidationService _identityValidationService;
        public AuthController(IMediator mediator, IAuthService authService, IUploadService uploadService, IIdentityValidationService identityValidationService)
        {
            _mediator = mediator;
            _authService = authService;
            _uploadService = uploadService;
            _identityValidationService = identityValidationService;
        }

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <remarks>
        /// Next Step: POST /api/auth/send-otp
        /// UserType:
        /// 2 = Customer
        /// 3 = Designer
        /// 4 = Producer
        /// </remarks>
        [HttpPost("register")]
        [ProducesResponseType(typeof(UserRegisterationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Register(UserRegisterationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        /// <summary>
        /// Sends OTP code to the user's email.
        /// </summary>
        /// <remarks>
        /// Next Step: POST /api/auth/verify-otp
        /// </remarks>
        [HttpPost("send-otp")]
        [ProducesResponseType(typeof(OtpResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> SendOtp(SendOtpCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// Verifies OTP code sent to user's email.
        /// </summary>
        /// <remarks>
        /// Next Step: Complete Registration Flow Based On UserType
        /// </remarks>
        [HttpPost("verify-otp")]
        [ProducesResponseType(typeof(OtpResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]

        public async Task<IActionResult> VerifyOtp(VerifyOtpCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// Completes customer registration profile.
        /// </summary>
        /// <remarks>
        /// Next Step:
        /// - If VERIFIED → POST /api/auth/send-Login-Link
        /// </remarks>

        [HttpPost("customer-registeration")]
        [ProducesResponseType(typeof(CustomerRegisteratonResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> CustomerRegisteration(CustomerRegisterationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// Completes producer registration profile.
        /// </summary>
        /// <remarks>
        /// Next Step:
        /// - If VERIFIED → POST /api/auth/send-Login-Link
        /// </remarks>
        [HttpPost("producer-registeration")]
        [ProducesResponseType(typeof(ProducerRegisterationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> ProducerRegisteration(ProducerRegisterrationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// Completes designer registration profile.
        /// </summary>
        /// <remarks>
        /// Next Step:
        /// - If VERIFIED → POST /api/auth/send-Login-Link
        /// </remarks>
        [HttpPost("designer-registeration")]
        [ProducesResponseType(typeof(DesignerRegisterationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DesignerRegisteration(DesignerRegesterationCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// Authenticates user and returns JWT token.
        /// </summary>
        /// <remarks>
        /// Supports:
        /// - Email/Password login
        /// - Account must be Active (not rejected or pending)
        /// </remarks>
        [HttpPost("login")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status423Locked)]
        public async Task<IActionResult> Login(UserLoginCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        [HttpPost("google-login")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }

        /// <summary>
        /// Forget Password Flow
        /// </summary>
        /// <remarks>
        /// When user clicks "Forget Password":
        /// STEP 1:
        /// POST /api/auth/send-otp
        /// → User enters email
        /// → Frontend must store the email temporarily
        /// STEP 2:
        /// POST /api/auth/verify-otp
        /// → User enters OTP code received in email
        /// → Frontend must store the email temporarily
        /// STEP 3:
        /// POST /api/auth/forget-password
        /// → frontend submits:
        ///   - Email (stored from step 1)
        ///   - Verified OTP (stored from step 2)
        /// → User submits:
        ///    - NewPassword
        ///    - ConfirmPassword
        /// FINAL STEP:
        /// → User can now login with the new password (goto login action)
        /// </remarks>

        [HttpPost("forget-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]

        public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
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
        /// <summary>
        /// Refreshes JWT access token.
        /// </summary>
        /// <remarks>
        /// Input: Refresh Token
        /// Output: New JWT Token
        /// </remarks>

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            var result = await _authService.JwtGenratedToken(refreshToken);
            return HandleResult(result);
        }
        /// <summary>
        /// Logs user out and revokes refresh token.
        /// </summary>

        [HttpPost("signout")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignOut(string refreshToken)
        {
            var result = await _authService.RevokeRefreshToken(refreshToken);
            return HandleResult(result);
        }

        /*        [HttpPost("upload-file")]
                [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
                [ProducesResponseType(StatusCodes.Status400BadRequest)]
                public async Task<IActionResult> UploadFile(IFormFile file)
                {
                    var result = await _uploadService.UploadFileAsync(file);
                    return HandleResult(result);
                }*/

        [HttpPost("identity-validation")]
        [ProducesResponseType(typeof(IdentityValidationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> IdentityValidation(IdentityValidationRequest request)
        {
            var result = await _identityValidationService.AnalyzeAsync(request.FrontImageUrl, request.BackImageUrl, request.SelfieImageUrl);
            return HandleResult(result);
        }
        /// <summary>
        /// Sends magic login link to verified users.
        /// </summary>
        /// <remarks>
        /// Only available for VERIFIED users.
        /// Next Step: Verify Magic Token
        /// </remarks>
        [HttpPost("send-login-link")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> SendLoginLink(SendLoginLinkCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
        /// <summary>
        /// Verifies magic login token and returns JWT.
        /// </summary>
        /// <remarks>
        /// Final authentication step.
        /// </remarks>
        [HttpPost("verify-magic-token")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]

        public async Task<IActionResult> VerifyMagicToken(VerifyMagicTokenCommand dto)
        {
            var result = await _mediator.Send(dto);
            return HandleResult(result);
        }
    }
}