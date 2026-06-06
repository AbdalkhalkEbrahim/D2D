using Domain.DTOs;
using Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UserRegisterationCommand: IRequest<string>
    {
        //validation

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; }

        [Required]
        [MaxLength(25)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*?]).*$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*?).")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ComfirmedPassword { get; set; }

        [Required]
        [EnumDataType(typeof(UserType))]
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
