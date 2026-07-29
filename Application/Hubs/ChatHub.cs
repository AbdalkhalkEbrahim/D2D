using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities.Chats;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Application.Hubs
{
    public class ChatHub : Hub
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;

        public ChatHub(D2DContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }


        [Authorize(Roles = "Producer, Customer")]
        public async Task CancelOffer(string senderId, string recieverId, string status) 
        {
            var task = new List<Task>
            {
               Clients.User(recieverId).SendAsync("requestCancelOffer", new {senderId, status}),
               Clients.User(senderId).SendAsync("requestCancelOffer", new {senderId, status})
            };
            await Task.WhenAll(task);
        }

        /* public async Task<string> sendMessage(string recieverId, MessageRequest message, MessageSender sender)
         {
             string senderId = Context.UserIdentifier!;

             var chatData = await _context.Chats
                 .Where(ch => (ch.CustomerID == recieverId && ch.ProducerID == senderId)
                           || (ch.CustomerID == senderId && ch.ProducerID == recieverId))
                 .Select(ch => new {
                     ch.ID,
                     ch.CustomerCount,
                     ch.CustomerLimit,
                     ch.ProdicerLimit,
                     ch.ProducerCount,
                     pAnon = ch.Producer.AnonName,
                     cAnon = ch.Customer.AnonName
                 }).FirstOrDefaultAsync();

             if (chatData == null)
                 return "no such chat found";

             if (sender == MessageSender.Customer && chatData.CustomerLimit == chatData.CustomerCount)
                 return "customer limit reached";//

             if (sender == MessageSender.Producer && chatData.ProdicerLimit == chatData.ProducerCount)
                 return "producer limit reached";//

             string messageContent = "";

             bool isImage = false;
             if (message.Image == null) 
                 messageContent = message.Text;
             else 
             {
                 isImage = true;
                 var imageNewFormat =  await _uploadService.ChangeFileFormat(new List<IFormFile> { message.Image });
                 messageContent = (await _uploadService.UploadFileAsync(imageNewFormat)).Value[0];
             }
             var messageToBeSent = new Message { Content = messageContent, Sender = sender, ChatID = chatData.ID, CreatedAt = DateTime.UtcNow };
             object returnedMessage = new { isImage , newMessage =  messageToBeSent};

             var chatToUpdate = new Chat { ID = chatData.ID };
             _context.Chats.Attach(chatToUpdate);

             if (sender == MessageSender.Customer)
             {
                 chatToUpdate.CustomerCount = chatData.CustomerCount + 1;
                 _context.Entry(chatToUpdate).Property(x => x.CustomerCount).IsModified = true;
             }
             else
             {
                 chatToUpdate.ProducerCount = chatData.ProducerCount + 1;
                 _context.Entry(chatToUpdate).Property(x => x.ProducerCount).IsModified = true;
             }

             string newSender = sender == MessageSender.Customer ? chatData.pAnon : chatData.cAnon;
             var notification = new Notification
             {
                 Title = "New Message Received",
                 Content = $"{newSender} waiting for your response",
                 NotificationsType = NotificationsType.RecieveOffer,
                 UserID = recieverId,
                 IsRead = false,
                 RefrenceUrl = $"/Chats/getChatById/{chatData.ID}?ownerId={recieverId}",
                 CreatedAt = DateTime.UtcNow
             };

             await Clients.User(recieverId).SendAsync("ReceiveMessage", new { returnedMessage, notification });
             await Clients.Caller.SendAsync("ReceiveMessage", message);

             await _context.Messages.AddAsync(messageToBeSent);
             await _context.Notifications.AddAsync(notification);
             await _context.SaveChangesAsync();

             return "message sent successfully";
         }*/

    }
}