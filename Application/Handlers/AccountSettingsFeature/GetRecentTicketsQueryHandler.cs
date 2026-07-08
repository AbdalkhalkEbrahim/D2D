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

namespace Application.Handlers.AccountSettingsFeature
{
    public class GetRecentTicketsQueryHandler : IRequestHandler<GetRecentTicketsQuery, Result<List<RecentTickets>>>
    {
        private readonly D2DContext _context;
        public GetRecentTicketsQueryHandler(D2DContext context)
        {
            _context=context;
        }
        public async Task<Result<List<RecentTickets>>> Handle(GetRecentTicketsQuery request, CancellationToken cancellationToken)
        {
            return _context.Tickets.Select(t => new RecentTickets
            {
                IssueType = t.IssueType.ToString(),
                CreatedAt = t.CreatedAt,
                Id = t.Id,
                Status = t.Status.ToString(),
            }).OrderByDescending(t => t.CreatedAt).ThenBy(t => t.Id).Take(3).ToList();
        }
    }
}
