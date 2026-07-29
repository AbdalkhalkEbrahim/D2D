using Application.Hubs;
using Application.Interfaces;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly D2DContext _context;

        public NotificationService(IHubContext<NotificationHub> notificationHub, D2DContext context)
        {
            _notificationHub = notificationHub;
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
            await _notificationHub.Clients.All.SendAsync("ReceiveNotification",new
            {
                notifications,
                publishedOffer
            });
        }

        public async Task SendProducerOfferNotification(string clientId, Guid producerOfferId)
        {
            var producerOffer = await _context.ProducerCustomerOffers.Select(po => new { po.ID, po.Price,po.Producer.AnonName,po.Producer.Rate }).FirstOrDefaultAsync(po => po.ID == producerOfferId);
            if (producerOffer == null)
            {
                throw new Exception("Producer offer not found.");
            }
            var notification = new Notification
            {
                Title = "New Producer Offer",
                Content = $"A new offer has been made by  {producerOffer.AnonName} with a price of: { producerOffer.Price}",
                NotificationsType = NotificationsType.RecieveOffer,
                UserID = clientId.ToString(),
                IsRead = false,
                RefrenceUrl = $"/Offers/ProducerOfferDetails/{producerOfferId}"
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            await _notificationHub.Clients.User(clientId.ToString()).SendAsync("ReceiveNotification", new
            {
                notification,
                producerOffer
            });
        }

        public async Task DeclineProducerOfferNotification(string producerId, string offerName)
        {
            var notification = new Notification
            {
                Title = "New Producer Offer",
                Content = $"Your offer on {offerName} has been declined",
                NotificationsType = NotificationsType.DeclineOffer,
                UserID = producerId.ToString(),
                IsRead = false,
                RefrenceUrl = $"/Offers/ProducerOfferDetails/{producerId}"
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            await _notificationHub.Clients.All./*User(producerId.ToString()).*/SendAsync("declineNotification", notification);
        }

        public async Task SendMessage(Notification notification, Notification? limit)
        {
            var sendTasks = new List<Task>
            {
                 _notificationHub.Clients.User(notification.UserID).SendAsync("ReceiveNotification", notification)
            };

            if (limit != null)
            {
                sendTasks.Add(_notificationHub.Clients.User(limit.UserID).SendAsync("ReceiveNotification", limit));
            }

            await Task.WhenAll(sendTasks);
        }
        public async Task ChangeStatus(Notification cNotification, Notification pNotification)
        {
            var task = new List<Task>
            {
               _notificationHub.Clients.User(cNotification.UserID).SendAsync("notifyChangeStatus", cNotification),
               _notificationHub.Clients.User(pNotification.UserID).SendAsync("notifyChangeStatus",pNotification)
            };
            await Task.WhenAll(task);
        }
        public async Task SendRequest(Notification cNotification, Notification pNotification)
        {
            var task = new List<Task>
            {
               _notificationHub.Clients.User(cNotification.UserID).SendAsync("notifyChangeStatus", cNotification),
               _notificationHub.Clients.User(pNotification.UserID).SendAsync("notifyChangeStatus",pNotification)
            };
            await Task.WhenAll(task);
        }
        public async Task CompleteDeal(Notification cNotification, Notification pNotification)
        {
            var task = new List<Task>
            {
               _notificationHub.Clients.User(cNotification.UserID).SendAsync("notifyChangeStatus", cNotification),
               _notificationHub.Clients.User(pNotification.UserID).SendAsync("notifyChangeStatus",pNotification)
            };
            await Task.WhenAll(task);
        }
    }
}
