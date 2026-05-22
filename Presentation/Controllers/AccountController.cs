using Domain.Entities.Customers;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        D2DContext c;


        public AccountController(UserManager<User> userManager, D2DContext d2DContext)
        {
            this.userManager = userManager;
            this.c = d2DContext;
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
    }
}
