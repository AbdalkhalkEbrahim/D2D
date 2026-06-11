using Application.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.SettingsFeature
{
    public class ChangePasswordCommand : IRequest<Result<string>>
    {
        [Required]
        public required string UserId { get; set; }

        [Required]
        [MaxLength(30)]
        public required string CurrentPassword { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*?]).*$",
            ErrorMessage = "New password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*?).")]
        public required string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
    }
}
