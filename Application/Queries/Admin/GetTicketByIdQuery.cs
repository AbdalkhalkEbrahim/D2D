using Application.Response;
using Domain.DTOs.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Admin
{
    public class GetTicketByIdQuery:IRequest<Result<TicketResponse>>
    {
        public int TicketId { get; set; }
    }
}
