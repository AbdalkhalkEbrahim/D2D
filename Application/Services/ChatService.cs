using Application.Hubs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IHubContext<ChatHub> _chatHub;

        public ChatService(IHubContext<ChatHub> chatHub)
        {
            _chatHub = chatHub;
        }
        public async Task SendMessage(string senderId, string recieverId, string message = "", string imageUrl="")
        {
            var task = new List<Task>
            {
                _chatHub.Clients.User(recieverId).SendAsync("ReceiveMessage", new {senderId,message , imageUrl }),
                _chatHub.Clients.User(senderId).SendAsync("ReceiveMessage", new {senderId, message , imageUrl })
            };
            await Task.WhenAll(task);
        }

    }
}
