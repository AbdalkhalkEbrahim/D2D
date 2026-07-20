using Application.Commands.ChatFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Offers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Handlers.ChatFeature
{
    public class ChangeActiveOfferStatusCommandHandler : IRequestHandler<ChangeActiveOfferStatusCommand, Result<string>>
    {
        private readonly D2DContext _context;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;
        public ChangeActiveOfferStatusCommandHandler(D2DContext context, IChatService chatService, INotificationService notificationService )
        {
            _context = context;
            _chatService = chatService;
            _notificationService = notificationService;
        }
        public async Task<Result<string>> Handle(ChangeActiveOfferStatusCommand request, CancellationToken cancellationToken)
        {
            var activeOffer = await _context.ActiveOfferLogs
                .Select(ao=>new {ao.ID, ao.Chat, ao.PublishedOfferID, ao.CustomOfferID,ao.Chat.CustomerID,CustomTotalAmount= ao.CustomerCustomOffer.ProducerCustomerOffer.Price,
                    PublishedTotalAmount= ao.CustomerPublishedOffer.ProducerCustomerOffers.FirstOrDefault(c => c.ProducerID == ao.Chat.ProducerID).Price,
                    ao.IsCustomOfferActive,ao.Chat.ProducerID,CustomDeposit=ao.CustomerCustomOffer.ProducerCustomerOffer.Diposit,
                    PublishedDeposit=ao.CustomerPublishedOffer.ProducerCustomerOffers.FirstOrDefault(c=>c.ProducerID==ao.Chat.ProducerID).Diposit,
                    ao.IsPublishedOfferActive, PublishedName = ao.CustomerPublishedOffer.Name, CustomName = ao.CustomerCustomOffer.Name ,ao.Step,ao.CreatedAt})
                .Where(ao => ao.Chat.ID == request.ChatID).OrderByDescending(ao=>ao.CreatedAt).FirstOrDefaultAsync();
            
            if(activeOffer == null) 
                return Result<string>.Failure(Messages.NotFound.WithTarget("Offer"));

            var name = activeOffer.PublishedOfferID != null ? activeOffer.PublishedName : activeOffer.CustomName;
            var cNotification = new Notification
            {
                NotificationsType = NotificationsType.ActveOfferStatuesChanged,
                Title = "Offer Status Changed",
                Content = $"The status of the offer {name} has been changed to {request.Step}.",
                RefrenceUrl = $"/chat/{request.ChatID}",
                UserID = activeOffer.Chat.CustomerID
            };
            var pNotification = cNotification;
            pNotification.UserID = activeOffer.Chat.ProducerID;

            var task = new List<Task>
            {
             _chatService.ChangeStatus(activeOffer.Chat.ProducerID, activeOffer.Chat.CustomerID, request.Step),
             _notificationService.ChangeStatus(cNotification, pNotification)
             };
            await Task.WhenAll(task);

            decimal deposit = 0m, total=0m;
            if (activeOffer.PublishedDeposit == null)
            {
                deposit = activeOffer.CustomDeposit;
                total=activeOffer.CustomTotalAmount-deposit;
            }
            else
            {
              deposit = activeOffer.PublishedDeposit;
              total=activeOffer.PublishedDeposit-deposit;
            }

            if(activeOffer.Step== ActiveOfferStatus.Negotiating.ToString())
            {
                await _context.Users
               .Where(u => u.Id == activeOffer.ProducerID)
               .ExecuteUpdateAsync(s => s.SetProperty(
                   u => u.Balance,
               u => u.Balance + (deposit - (deposit * 0.15m))
               ), cancellationToken);

               var newCustomerBalance =await _context.Users
                 .Where(u => u.Id == activeOffer.CustomerID && u.Balance>=total)
                 .ExecuteUpdateAsync(s => s.SetProperty(
                     u => u.Balance,
                 u => u.Balance - total
                 ), cancellationToken);

                if(newCustomerBalance==0)
                    return Result<string>.Failure(new Error("BadRequest","The balance is less than price"));

                await _context.Users
                .Where(u => u.UserType == UserType.Admin)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.Balance,
                u => u.Balance + total * 0.15m
                ), cancellationToken);
            }
               

            var active = new ActiveOfferLogs { ID = activeOffer.ID };
            _context.Attach(active);
            _context.Entry(active).Property(a => a.UpdatedAt).CurrentValue = DateTime.UtcNow;
            _context.Entry(active).Property(a => a.UpdatedAt).IsModified = true;

            var newActivelog = new ActiveOfferLogs
            {
                Step = request.Step,
                ChatID = request.ChatID,
                CreatedAt = DateTime.UtcNow,
                PublishedOfferID = activeOffer.PublishedOfferID,
                CustomOfferID = activeOffer.CustomOfferID,
                IsCustomOfferActive = activeOffer.IsCustomOfferActive,
                IsPublishedOfferActive = activeOffer.IsPublishedOfferActive,
            };

            _context.Add(newActivelog);
            _context.AddRange(new List<Notification> { cNotification, pNotification });
            await _context.SaveChangesAsync();

            return active.Step;
        }
    }
}
