using Application.Commands;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Domain.Entities.Customers;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly D2DContext c;
        private readonly IMediator _mediator;
        private readonly Cloudinary _cloudinary;

        public AccountController(UserManager<User> userManager, D2DContext d2DContext, IMediator mediator, IOptions<CloudinarySettings> config)
        {
            this.userManager = userManager;
            this.c = d2DContext;
            this._mediator = mediator;
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }
/*        [HttpPost("Register")]
        public async Task<ActionResult<GeneralResponse>> Register(RegisterDTO registerUser)
        {
            var c = new Customer();
            var ApplicationUser = new User();
            ApplicationUser.UserName = registerUser.Name;
            ApplicationUser.Email = registerUser.Email;
            var res = await userManager.CreateAsync(ApplicationUser, registerUser.Password);

            if (ModelState.IsValid)
            {
                if (res.Succeeded)
                {

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
                    var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes((token)));
                    //var encodedEmail = WebUtility.UrlEncode(ApplicationUser.Email);

                    new VerificationConfirmation().SendEmailAsync(ApplicationUser.Email, "Verification", "Hello 2llaa");
                    return new GeneralResponse { Message = "Created Successfully", Status = 201 };
                }


                foreach (var erorr in res.Errors)
                    ModelState.AddModelError("", erorr.Description);
            }
            return new GeneralResponse { Message = "Can't Register", Status = 400, Data = ModelState };
        }*/
        [HttpPost("AddCustomer")]
        public IActionResult Add(string name, string email, string password,string city)
        {
            var customer = new Customer() { FirstName = name, Email = email, PasswordHash = password,};
            customer.Addresses = new HashSet<Address> { new Address { City = city, AppartmentNo = "", Goverate="",  Street="" } };
            c.Customers.Add(customer);
            c.SaveChanges();
            return Ok();
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp(SendOtpCommand dto)
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
        [HttpPost("delete")]
        public async Task<DeletionResult> DeleteFileAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image // حدد نوع الملف (Image, Video, Raw)
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result; // النتيجة ستكون result.Result == "ok" في حال النجاح
        }
    }
}
