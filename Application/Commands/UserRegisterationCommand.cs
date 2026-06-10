using Application.Response;
using Domain.DTOs;
using Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class UserRegisterationCommand: IRequest<Result<UserRegisterationResponse>>
    {
        //validation

        [Required]
        [StringLength(30)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(30)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*?]).*$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*?).")]
        public required string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public required string ComfirmedPassword { get; set; }

        [Required]
        [EnumDataType(typeof(UserType))]
        [Range(2, 4)]
        public UserType UserType { get; set; }

        [Required]
        [Range(1, 31)]
        public int Day { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Range(1956, 2008)]
        public int Year { get; set; }
    }
}
