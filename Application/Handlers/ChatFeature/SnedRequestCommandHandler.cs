using Application.Commands.ChatFeature;
using Application.Hubs;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Chats;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
namespace Application.Handlers.ChatFeature
{
    public class SnedRequestCommandHandler : IRequestHandler<SendRequestCommand, Result<bool>>
    {
        private readonly D2DContext _context;
        private readonly INotificationService _notificationService;
        private readonly IChatService _chatService;

        public SnedRequestCommandHandler(D2DContext context, IChatService chatService, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
            _chatService = chatService;
        }
        public async Task<Result<bool>> Handle(SendRequestCommand request, CancellationToken cancellationToken)
        {
            var chat = await _context.Chats.Include(ch=>ch.RequestsLogs)
                .FirstOrDefaultAsync(ch => ch.ID == request.ChatId && (ch.CustomerID == request.UserId || ch.ProducerID == request.UserId));
               
            if (chat == null)
                return Result<bool>.Failure(Messages.NotFound.WithTarget("Chat"));

            string reciever = request.UserId == chat.ProducerID ? chat.CustomerID : chat.ProducerID;
            string sender = request.UserId == chat.ProducerID ? chat.ProducerID : chat.CustomerID;
            var Notification1 = new Notification
            {
                NotificationsType = NotificationsType.DeclineOffer,
                Title = "Request sent",
                Content = $"You've requested {request.request.ToString()} to change the offer with {reciever} ",
                RefrenceUrl = $"/chat/{chat.ID}",
                UserID = sender
            };

            var Notification2 = new Notification
            {
                NotificationsType = NotificationsType.DeclineOffer,
                Title = "Request sent",
                Content = $"You've received a request from {sender} which is {request.request.ToString()}",
                RefrenceUrl = $"/chat/{chat.ID}",
                UserID = reciever
            };

            var task = new List<Task>
            {
               _notificationService.SendRrequest(Notification1, Notification2),
               _chatService.SendRequest(sender, reciever, request.request.ToString())
            };
            await Task.WhenAll(task);

            var log = new RequestsLogs { ChatId = chat.ID, request = request.request, 
                UserId = request.UserId == chat.ProducerID ? chat.ProducerID : chat.CustomerID };
            chat.RequestsLogs.Add(log);

            if(request.request == CancelationRequest.Accepted)
            {
                chat.IsClosed = true;
                _context.Update(chat);
            }
            await _context.AddAsync(chat);
            await _context.SaveChangesAsync();
            
            return chat.IsClosed;
        }

    }
}
