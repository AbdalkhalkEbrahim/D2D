using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;

namespace Application.Handlers.Admin
{
    public class GetAllTicketsQueryHandler : IRequestHandler<GetAllTicketsQuery, Result<List<TicketResponse>>>
    {
        private readonly D2DContext _context;
        public GetAllTicketsQueryHandler(D2DContext context)
        {
            _context= context;
        }
        public async Task<Result<List<TicketResponse>>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            int allCount, resolvedCount, openCount, inProgressCount;
            var tickets = _context.Tickets.Select(t => new { t.CreatedAt, t.Id, t.IssueType, t.Status, t.User.FirstName, t.User.LastName, t.Description });
            var count = tickets.GroupBy(_ => 1).Select(t => new
            {
                AllCount = tickets.Count(),
                ResolvedCount = tickets.Count(t => t.Status == TicketStatus.Resolved),
                OpenCount = tickets.Count(t => t.Status == TicketStatus.Open),
                InProgressCount = tickets.Count(t => t.Status == TicketStatus.InReview)
            }).FirstOrDefault();
            if (count == null)
                return Result<List<TicketResponse>>.Failure(Messages.NotFound.WithTarget("Default"));

            allCount = count.AllCount;
            resolvedCount = count.ResolvedCount;
            openCount = count.OpenCount;
            inProgressCount = count.InProgressCount;

            tickets = request.isTheNewst ? tickets.OrderByDescending(t => t.CreatedAt).ThenBy(o => o.Id) : tickets.OrderBy(t => t.CreatedAt).ThenBy(o => o.Id);

            if (request.PageNum <= 0)
                request.PageNum = 1;
            if (request.PageSize <= 0)
                request.PageSize = 6;

            tickets = tickets.Skip((request.PageNum - 1) * request.PageSize).Take(request.PageSize);

            return tickets.Select(t => new TicketResponse
            {
                Id = t.Id,
                Description = t.Description,
                IssueType = t.IssueType.ToString(),
                Status = t.Status.ToString(),
                SubmitedBy = t.FirstName + " " + t.LastName,
                CreatedAt=t.CreatedAt,
            }).ToList(); 

        }
    }
}
