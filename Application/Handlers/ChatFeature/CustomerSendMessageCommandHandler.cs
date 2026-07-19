using Application.Commands.ChatFeature;
using Application.Hubs;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Chats;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ChatFeature
{
    public class CustomerSendMessageCommandHandler : IRequestHandler<CustomerSendMessageCommand, Result>
    {
        private readonly D2DContext _context;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;
        public CustomerSendMessageCommandHandler(D2DContext context, IChatService chatService, INotificationService notificationService)
        {
            _context = context;
            _chatService = chatService;
            _notificationService = notificationService;
        }
        public async Task<Result> Handle(CustomerSendMessageCommand request, CancellationToken cancellationToken)
        {
            var chat = await _context.Chats.Where(c => c.ID == request.ChatId&&!c.Customer.IsDeleted)
                .Select(ch => new {ch.CustomerLimit, ch.CustomerCount, ch.ID, customerAnonName = ch.Customer.AnonName,producerAnonName = ch.Producer.AnonName, ch.ProducerID, ch.CustomerID, ch.IsClosed})
                .FirstOrDefaultAsync();

            if (chat == null)
                return Result.Failure(Messages.NotFound.WithTarget("Chat"));

            if (chat.CustomerCount >= chat.CustomerLimit)
            {
                var endedChat = new Chat { ID = chat.ID, IsClosed = true };
                _context.Attach(endedChat);
                _context.Entry(endedChat).Property(ch => ch.IsClosed).IsModified = true;
                await _context.SaveChangesAsync();
                return Result.Failure(Messages.Forbidden.WithTarget("ChatLimit"));
            }

            Notification? limit = null;
            if((chat.CustomerCount * 1.0 / chat.CustomerLimit) * 100 > 90)
            {
                limit = new Notification
                {
                    Title = $"Chat Limit about to hit",
                    Content = $"Your messages limit with {chat.producerAnonName} reached {(chat.CustomerCount*1.0/chat.CustomerLimit)*100}% ",
                    NotificationsType = NotificationsType.ChatLimit,
                    UserID = chat.CustomerID,
                    IsRead = false,
                    RefrenceUrl = $"/Chats/getChatById/{chat.ID}?ownerId={chat.CustomerID}",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Add(limit);
            }
            var newMessage = new Message
            {
                ChatID = chat.ID,
                Sender = MessageSender.Customer,
                Content = new List<string> { request.MessageText },
                CreatedAt = DateTime.UtcNow
            };

            var notification = new Notification
            {
                Title = "New Message Received",
                Content = $"{chat.customerAnonName} waiting for your response",
                NotificationsType = NotificationsType.RecieveMessage,
                UserID = chat.ProducerID,
                IsRead = false,
                RefrenceUrl = $"/Chats/getChatById/{chat.ID}?ownerId={chat.ProducerID}",
                CreatedAt = DateTime.UtcNow
            };

            var tasks = new List<Task>
            {
                _chatService.SendMessage(chat.CustomerID, chat.ProducerID, request.MessageText),
                _notificationService.SendMessage(notification, limit)
            };
            await Task.WhenAll(tasks);

            var chatEntity = new Chat { ID = chat.ID, CustomerCount = chat.CustomerCount };
            _context.Attach(chatEntity);
            _context.Entry(chatEntity).Property(c => c.CustomerCount).CurrentValue = chat.CustomerCount + 1;
            _context.Entry(chatEntity).Property(c => c.CustomerCount).IsModified = true;
            
            _context.Add(notification);
            _context.Add(newMessage);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
