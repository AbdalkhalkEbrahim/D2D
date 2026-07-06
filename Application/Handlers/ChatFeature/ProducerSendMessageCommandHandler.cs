using Application.Commands.ChatFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Chats;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Application.Handlers.ChatFeature
{
    public class ProducerSendMessageCommandHandler : IRequestHandler<ProducerSendMessageCommand, Result>
    {
        private readonly D2DContext _context;
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;
        private readonly IUploadService _uploadService;
        public ProducerSendMessageCommandHandler(D2DContext context, INotificationService notificationService, IChatService chatService, IUploadService uploadService)
        {
            _context = context;
            _notificationService = notificationService;
            _chatService = chatService;
            _uploadService = uploadService;
        }
        public async Task<Result> Handle(ProducerSendMessageCommand request, CancellationToken cancellationToken)
        {
            if(request.MessageText == null && request.MessageImageUrl == null)
                return Result.Failure(Messages.BadRequest.WithTarget("NullValue"));

            var chat = await _context.Chats.Where(c => c.ID == request.ChatId)
               .Select(ch => new { ch.ProducerCount, ch.ProducerLimit, ch.ID, customerAnonName = ch.Customer.AnonName, producerAnonName = ch.Producer.AnonName, ch.ProducerID, ch.CustomerID, ch.IsClosed })
               .FirstOrDefaultAsync();

            if (chat == null)
                return Result.Failure(Messages.NotFound.WithTarget("Chat"));

            if (chat.ProducerCount >= chat.ProducerLimit)
            {
                var endedChat = new Chat { ID = chat.ID, IsClosed = true };
                _context.Attach(endedChat);
                _context.Entry(endedChat).Property(ch => ch.IsClosed).IsModified = true;
                await _context.SaveChangesAsync();

                return Result.Failure(Messages.Forbidden.WithTarget("ChatLimit"));
            }

            Notification? limit = null;
            if ((chat.ProducerCount * 1.0 / chat.ProducerLimit) * 100 > 90)
            {
                limit = new Notification
                {
                    Title = $"Chat Limit about to hit",
                    Content = $"Your messages limit with {chat.customerAnonName} reached {(chat.ProducerCount * 1.0 / chat.ProducerLimit) * 100}% ",
                    NotificationsType = NotificationsType.ChatLimit,
                    UserID = chat.ProducerID,
                    IsRead = false,
                    RefrenceUrl = $"/Chats/getChatById/{chat.ID}?ownerId={chat.ProducerID}",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Add(limit);
            }

            var newMessage = new Message
            {
                ChatID = chat.ID,
                Sender = MessageSender.Producer,
                Content = new List<string> { request.MessageText??"" },
                CreatedAt = DateTime.UtcNow
            };


            string imageUrl = "";
            if (request.MessageImageUrl != null)
            {
                var formatedImage = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.MessageImageUrl});
                var uploadResult = await _uploadService.UploadFileAsync(formatedImage);
                if (!uploadResult.IsSuccess)
                {
                    return Result.Failure(Messages.NotFound.WithTarget("Default"));
                }
                newMessage.Content.Add(uploadResult.Value[0]);
                imageUrl = uploadResult.Value[0];
            }

            var notification = new Notification
            {
                Title = "New Message Received",
                Content = $"{chat.producerAnonName} waiting for your response",
                NotificationsType = NotificationsType.RecieveMessage,
                UserID = chat.CustomerID,
                IsRead = false,
                RefrenceUrl = $"/Chats/getChatById/{chat.ID}?ownerId={chat.CustomerID}",
                CreatedAt = DateTime.UtcNow
            };

            var tasks = new List<Task>
            {
                _chatService.SendMessage(chat.ProducerID, chat.CustomerID, newMessage.Content[0], imageUrl),
                _notificationService.SendMessage(notification, limit)
            };
            await Task.WhenAll(tasks);

            var chatEntity = new Chat { ID = chat.ID, ProducerCount = chat.ProducerCount };
            _context.Attach(chatEntity);
            _context.Entry(chatEntity).Property(c => c.ProducerCount).CurrentValue = chat.ProducerCount + 1 + (request.MessageText!= null && request.MessageImageUrl != null? 1 : 0);
            _context.Entry(chatEntity).Property(c => c.ProducerCount).IsModified = true;

            _context.Add(notification);
            _context.Add(newMessage);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
