using Application.Commands.AccountSettingsFeature;
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

namespace Application.Handlers.AccountSettingsFeature
{
    public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand, Result>
    {
        private readonly D2DContext _context;
        public AddTicketCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(AddTicketCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
                return Result.Failure(Messages.NotFound.WithTarget("User"));
            var ticket = new Tickets
            {
                UserId = request.UserId,
                Description = request.Description,
                IssueType = request.IssueType,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };
            _context.Add(ticket);
            await _context.SaveChangesAsync();
            return Result.Success();
        }
    }
}
