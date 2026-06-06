using Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UserLoginCommand:IRequest<object>
    {
        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; }
        [Required]
        [MaxLength(25)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^&*?]).*$",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character (!@#$%^&*?).")]
        public string Password { get; set; }
    }
}
