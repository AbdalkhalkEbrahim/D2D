using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin
{
    public class SoftDeleteUserCommand:IRequest<Result>
    {
        public string UserId { get; set; }
    }
}
