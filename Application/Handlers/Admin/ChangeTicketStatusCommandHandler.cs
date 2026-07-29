using Application.Commands.Admin;
using Application.Response;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Admin
{
    public class ChangeTicketStatusCommandHandler : IRequestHandler<ChangeTicketStatusCommand, Result>
    {
        private readonly D2DContext _context;
        public ChangeTicketStatusCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ChangeTicketStatusCommand request, CancellationToken cancellationToken)
        {
            
            var ticket =await _context.Tickets.Select(t => new { t.Id, t.Status }).FirstOrDefaultAsync(t => t.Id == request.TicketId);

            if (ticket == null)
                return Result.Failure(Messages.NotFound.WithTarget("Default"));
            var editedTicket = new Tickets { Id = ticket.Id, Status = request.Status };

            _context.Attach(editedTicket);
            _context.Entry(editedTicket).Property(t => t.Status).IsModified = true;
           await _context.SaveChangesAsync();

            return Result.Success();

        }
    }
}
