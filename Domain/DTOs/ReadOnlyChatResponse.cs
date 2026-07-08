using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ReadOnlyChatResponse
    {
        public Guid DesignId { get; set; }
        public string OfferName { get; set; }   
        public List<string> DesignImages { get; set; }
        public List<ReadOnlyMessagesResponse>Messages { get; set; }
        public int ChatId { get; set; }
        public Guid PublishedOfferId { get; set; }

    }
}
