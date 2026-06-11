using Application.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class VerifyOtpCommand:IRequest<Result<bool>>
    {
        [Required]
        public string UserId { get; set; }  
        [Required]
        [RegularExpression("^[A-Za-z0-9!@#$%^&*?]{6}$", ErrorMessage = "Invalid Otp format.")]
        public string Otp { get; set; }
    }
}
