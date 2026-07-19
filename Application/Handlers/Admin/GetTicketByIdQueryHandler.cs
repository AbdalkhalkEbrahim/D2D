using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
using Infrastructure.Data.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Admin
{
    public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, Result<TicketResponse>>
    {
        private readonly D2DContext _context;
        public GetTicketByIdQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<TicketResponse>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var ticket = _context.Tickets.Where(u=>!u.User.IsDeleted).Select(t=>new { t.CreatedAt,t.Id,t.IssueType,t.Status,t.User.FirstName,t.User.LastName,t.Description }).FirstOrDefault(t => t.Id == request.TicketId);
            if(ticket == null)
                return Result<TicketResponse>.Failure(Messages.NotFound.WithTarget("Default"));
            return new TicketResponse
            {
                Id = ticket.Id,
                Description = ticket.Description,
                IssueType = ticket.IssueType.ToString(),
                Status = ticket.Status.ToString(),
                SubmitedBy = ticket.FirstName + " " + ticket.LastName,
                CreatedAt=ticket.CreatedAt,
            };
        }
    }
}
