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
    public class GetAllTicketsQuery: IRequest<Result<List<TicketResponse>>>
    {
        public int? TicketId { get; set; }
        public bool isAll { get; set; }
        public bool isResolved { get; set; }
        public bool isOpen { get; set; }
        public bool isInProgress { get; set; }
        public bool isTheNewst { get; set; }
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
