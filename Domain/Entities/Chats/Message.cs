using Domain.Entities.Shared;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Chats
{
    public class Message:Audits
    {
        public int ID { get; set; }
        public string? Content { get; set; }
        public MessageSender Sender { get; set; }
        public Chat Chat { get; set; }
        [ForeignKey("Chat")]
        public int ChatID { get; set; }

    }
}
