using Application.Commands.NotificationFeature;
using Application.Response;
using Domain.DTOs.NotificationDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.NotificationFeature
{
    public class ReadNotificationCommandHandler : IRequestHandler<ReadNotificationCommand, Result<NotificationResponse>>
    {
        private readonly D2DContext _context;
        public ReadNotificationCommandHandler(D2DContext context)
        {
            _context = context;
        }

        public async Task<Result<NotificationResponse>> Handle(ReadNotificationCommand request, CancellationToken cancellationToken)
        {    
            var notification = await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(n=>n.ID==request.NotificationId && n.UserID==request.UserId);
            if (notification == null) 
                return Result<NotificationResponse>.Failure(Messages.NotFound.WithTarget("Notification"));
            if(!notification.IsRead)
            {
                notification.IsRead = true;
                _context.Attach(notification);
                _context.Entry(notification).Property(n => n.IsRead).IsModified = true; 
                await _context.SaveChangesAsync();
            }

            return new NotificationResponse
            {
                Id = notification.ID,
                Title = notification.Title,
                Message = notification.Content,
                CreatedAt = notification.CreatedAt,
                referenceUrl = notification.RefrenceUrl,
                IsRead = notification.IsRead
            };
        }
    }
}
