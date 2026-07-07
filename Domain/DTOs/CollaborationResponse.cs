using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CollaborationResponse
    {
        public Guid DesignId { get; set; }
        public List<string> DesignImages { get; set; }
        public string PublishOfferName { get; set; }
        public string CustomerName { get; set; }
        public string ProducerName { get; set; }
        public string CustomerImage { get; set; }
        public string ProducerImage { get; set; }
        public string Deposit {  get; set; }
        public decimal Amount { get; set; }
        public Dictionary<string,Tuple<int,int>> Steps { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ChatId { get; set; }
        public string Status { get; set; }
        public int  AllCount {get; set;}
        public int  CompletedCount {get; set;}
        public int  ClosedCount {get; set;}
        public int  PendingCount {get; set;}

    }
}
