using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class ForgetPasswordCommand:IRequest<string>
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z0-9!@#$%^&*?]{6}$", ErrorMessage = "Invalid Otp format.")]
        public string Otp { get; set; }
        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*?]).*$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*?).")]
        public string NewPassword { get; set; }
        [Required]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
