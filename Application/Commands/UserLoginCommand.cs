using Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UserLoginCommand:IRequest<JwtToken>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
