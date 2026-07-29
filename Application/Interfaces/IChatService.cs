using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IChatService
    {
        public Task SendMessage(string senderId, string recieverId, string message = "", string imageUrl = "");
        public Task ChangeStatus(string senderId, string recieverId, string status);

        public Task SendRequest(string senderId, string recieverId, string status);

    }
}
