using Application.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.RegisterationFeature
{
    public class ForgetPasswordCommand : IRequest<Result<string>>
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z0-9!@#$%^&*?]{6}$", ErrorMessage = "Invalid Otp format.")]
        public required string Otp { get; set; }
        /*[Required]
        [MaxLength(30)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*?]).*$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*?).")]
*/        public required string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public required string ConfirmPassword { get; set; }
    }
}
