using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Chat
{
    public class ChatWithMessagesResponse
    {
        public List<MessagesResponse> Messages { get; set; }
        public string AnonName { get; set; }
        public string? Step { get; set; }
    }
}
