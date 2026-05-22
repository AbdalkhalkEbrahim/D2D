using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Chats.AiModel
{
    public class ModelChatMessage
    {
        public int ID { get; set; }
        public string? Text { get; set; }
        public string? ImgUrl { get; set; }
        public int Limit { get; set; } = 20;
        public int LimitCounter { get; set; }
        public MessageSender Sender { get; set; }
        public virtual ModelChat ModelChat { get; set; }
        [ForeignKey(nameof(ModelChat))]
        public int ModelChatID { get; set; }
    }
}
