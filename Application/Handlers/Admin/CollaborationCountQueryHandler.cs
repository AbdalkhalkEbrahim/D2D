using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Handlers.Admin
{
    public class CollaborationCountQueryHandler : IRequestHandler<CollaborationCountQuery, Result<ActiveCollaborationCountResponse>>
    {
        private readonly D2DContext _context;
        public CollaborationCountQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ActiveCollaborationCountResponse>> Handle(CollaborationCountQuery request,CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var lastMonth = now.AddMonths(-1);
            var twoMonthsAgo = now.AddMonths(-2);

            var collaboration = await _context.ActiveOfferLogs
                    .Where(c => c.CreatedAt >= twoMonthsAgo)
                    .GroupBy(_ => 1)
                    .Select(g => new
                    {
                        PreviousMonth = g
                            .Where(x => x.CreatedAt >= twoMonthsAgo && x.CreatedAt < lastMonth)
                            .Select(x => x.CustomOfferID)
                            .Distinct()
                            .Count(),

                        CurrentMonth = g
                            .Where(x => x.CreatedAt >= lastMonth)
                            .Select(x => x.CustomOfferID)
                            .Distinct()
                            .Count()
                    })
                    .FirstOrDefaultAsync(cancellationToken);

            if (collaboration == null)
                return new ActiveCollaborationCountResponse();

            double collaborationPercentage = collaboration.PreviousMonth == 0
                ? (collaboration.CurrentMonth > 0 ? 100 : 0)
                : ((collaboration.CurrentMonth - collaboration.PreviousMonth) * 100.0)
                    / collaboration.PreviousMonth;

            return new ActiveCollaborationCountResponse
            {
                ActiveCollaborationCount = collaboration.CurrentMonth,
                Percentage = Math.Round(collaborationPercentage, 2)
            };
        }
    }
}
