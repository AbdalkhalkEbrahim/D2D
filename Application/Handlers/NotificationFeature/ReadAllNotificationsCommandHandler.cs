using Application.Commands.NotificationFeature;
using Application.Response;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.NotificationFeature
{
    public class ReadAllNotificationsCommandHandler : IRequestHandler<ReadAllNotificationsCommand, Result>
    {
        private readonly D2DContext _context;
        public ReadAllNotificationsCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ReadAllNotificationsCommand request, CancellationToken cancellationToken)
        {
            var notifications = await _context.Notifications.AsNoTracking().Where(n => n.UserID == request.UserId && !n.IsRead).ToListAsync();

            if (!notifications.Any())
                return Result.Failure(Messages.NotFound.WithTarget("Notification"));


            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                _context.Attach(notification);
                _context.Entry(notification).Property(n => n.IsRead).IsModified = true;
            }
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
