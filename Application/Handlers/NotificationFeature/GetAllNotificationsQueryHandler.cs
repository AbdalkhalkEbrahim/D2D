using Application.Queries.NotificationFeature;
using Application.Response;
using Domain.DTOs.NotificationDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.NotificationFeature
{
    public class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, Result<List<NotificationResponse>>>
    {
        private readonly D2DContext _context;
        public GetAllNotificationsQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<NotificationResponse>>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
        {
           var notifications =await _context.Notifications.AsNoTracking().Where(n => n.UserID == request.UserId).Select(n => new NotificationResponse
           {
               Id = n.ID,
               Title = n.Title,
               Message = n.Content,
               CreatedAt = n.CreatedAt,
               IsRead = n.IsRead
           }).OrderByDescending(n => n.CreatedAt).ToListAsync();

            if (!notifications.Any())
                return Result<List<NotificationResponse>>.Failure(Messages.NotFound.WithTarget("Notification"));

            return notifications;
        }
    }
}
