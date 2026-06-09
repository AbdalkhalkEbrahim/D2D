using Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class VerifyMagicTokenCommand:IRequest<JwtToken>
    {
        public string Token { get; set; }   
    }
}
