using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Commands
{
    public class SendOtpCommand:IRequest<string>
    {
        [Required]
        [EmailAddress]
        [MaxLength(30)]
        public string Email { get; set; }
    }
}
