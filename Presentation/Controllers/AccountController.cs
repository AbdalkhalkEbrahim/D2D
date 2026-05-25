using Application.Commands;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Domain.Entities.Customers;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Domain.Settings;
using Microsoft.Extensions.Options;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        private readonly Cloudinary _cloudinary;


        public AccountController(IMediator mediator, IOptions<CloudinarySettings> config)
        {
            _mediator = mediator;
            var account = new Account(
               config.Value.CloudName,
               config.Value.ApiKey,
               config.Value.ApiSecret // ?? ApiSecret ??? ???? ??????
           );

            _cloudinary = new Cloudinary(account);
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





        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadResult = new ImageUploadResult();

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }

            if (uploadResult.Error != null)
            {
                return BadRequest($"Cloudinary Error: {uploadResult.Error.Message}");
            }

            string imageUrl = uploadResult.SecureUrl.ToString();
            return Ok(new { Url = imageUrl });
        }
    }
}
