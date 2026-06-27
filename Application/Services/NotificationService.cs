using Application.Interfaces;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly D2DContext _context;

        public NotificationService(IHubContext<NotificationHub> hubContext, D2DContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }
       
        public async Task SendPuplishedDesignNotificationAsync(Guid customerPOfferId)
        {
            var publishedOffer =await _context.CustomerPublishedOffers.Select(d => new {d.ID,d.Category,d.MaxPrice,d.Amount,d.Duration,d.PrintingType}).FirstOrDefaultAsync(o => o.ID == customerPOfferId);
            var producersIds = await _context.Producers.Select(p=>p.Id).ToListAsync();
            if (publishedOffer == null)
            {
                throw new Exception("Published offer not found.");
            }
            var notifications =producersIds.Select(p=> new Notification
            {
                Title = "New Published Design Offer",
                Content = "A new design offer has been published in the category: " + publishedOffer.Category + ". The maximum price is: " + publishedOffer.MaxPrice + ". The amount is: " + publishedOffer.Amount + ". The duration is: " + publishedOffer.Duration + ". The printing type is: " + publishedOffer.PrintingType ,
                NotificationsType = NotificationsType.PulishDesign,
                UserID=p,
                IsRead = false,
                RefrenceUrl = $"/Designs/PublishedOfferDetails/{customerPOfferId}"
            }).ToList();
            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.Group("ProducersGroup").SendAsync("ReceiveNotification",new
            {
                notifications,
                publishedOffer
            }
                );
        }
    }
}
