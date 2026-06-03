using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class VerifyOtpCommand:IRequest<bool>
    {
        public string UserId { get; set; }  
        public string Otp { get; set; }
    }
}
