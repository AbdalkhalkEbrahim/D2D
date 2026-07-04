using Application.Commands.ChatFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Offers;
using Domain.Entities.Shared;
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
                .Select(ao=>new {ao.ID, ao.Chat, ao.PublishedOfferID, ao.CustomOfferID, ao.IsCustomOfferActive,
                    ao.IsPublishedOfferActive, PublishedName = ao.CustomerPublishedOffer.Name, CustomName = ao.CustomerCustomOffer.Name})
                .Where(ao => ao.Chat.ID == request.ChatID).FirstOrDefaultAsync();
            
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
