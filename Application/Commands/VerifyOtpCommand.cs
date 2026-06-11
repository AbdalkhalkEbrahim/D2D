using Application.Response;
using Domain.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class VerifyOtpCommand:IRequest<Result<OtpResponse>>
    {
        [Required]
        public required string UserId { get; set; }  
        [Required]
        [RegularExpression("^[A-Za-z0-9!@#$%^&*?]{6}$", ErrorMessage = "Invalid Otp format.")]
        public required string Otp { get; set; }
    }
}
