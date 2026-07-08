using Application.Response;
using Domain.Enums.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Admin
{
    public class ChangeTicketStatusCommand:IRequest<Result>
    {
        public int TicketId { get; set; }
        public TicketStatus Status { get; set; }
    }
}
