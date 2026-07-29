using Domain.Entities.Offers;
using Domain.Enums.Status;
using Domain.Enums.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Chat
{
    public class ChatsResponse
    {
        public int ChatId { get; set; }
        public string DesignImageUrl { get; set; }
        public string AnonName { get; set; }
        public DateTime? LastMessageAgo { get; set; }
        public string? LastMessage { get; set; }
        public string OfferStatus { get; set; }
        public bool IsRead { get; set; }
    }
}
