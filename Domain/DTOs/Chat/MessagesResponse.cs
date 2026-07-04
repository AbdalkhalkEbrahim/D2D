using Domain.Enums.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Chat
{
    public class MessagesResponse
    {
        public int ID { get; set; }
        public List<string> Message { get; set; }
        public MessageSender Sender { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
