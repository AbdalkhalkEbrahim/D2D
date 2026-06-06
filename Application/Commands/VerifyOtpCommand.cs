using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class VerifyOtpCommand:IRequest<bool>
    {
        [Required]
        public string UserId { get; set; }  
        [Required]
        [RegularExpression("^[A-Za-z0-9!@#$%^&*?]{6}$", ErrorMessage = "Invalid Otp format.")]
        public string Otp { get; set; }
    }
}
