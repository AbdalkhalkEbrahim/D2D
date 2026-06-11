using Application.Response;
using Domain.DTOs.AuthDtos;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.RegisterationFeature
{
    public class UserLoginCommand : IRequest<Result<JwtToken>>
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }
        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^(?=.*)(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*?]).*$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*?).")]
        public required string Password { get; set; }
    }
}
