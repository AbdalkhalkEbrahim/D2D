using Application.Response;
using Domain.DTOs.RegisterationDtos;
using Domain.Enums.Types;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.RegisterationFeature
{
    public class UserRegisterationCommand : IRequest<Result<UserRegisterationResponse>>
    {
        //validation

        [Required]
        [RegularExpression("^[a-zA-O-Z-a-z]{1,30}$", ErrorMessage ="Invalid name format")]
        public required string FirstName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-O-Z-a-z]{1,30}$", ErrorMessage = "Invalid name format")]

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
