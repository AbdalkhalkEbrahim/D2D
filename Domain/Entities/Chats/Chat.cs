using Domain.Entities.Customers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Chats
{
    public class Chat : Audits
    {
        public int ID { get; set; }
        public string Name { get;  set; }
        public virtual Producer Producer { get; set; }
        public virtual Customer Customer { get; set; }
        public int ProducerLimit { get; private set; } = 150;
        public int CustomerLimit { get; private set; } = 100;
        public int ProducerCount { get; set; }
        public int CustomerCount { get; set; }
        public ICollection<RequestsLogs> RequestsLogs { get; set; }
        public bool IsClosed { get; set; } = false;
       
        [ForeignKey(nameof(Customer))]
        public string CustomerID { get; set; }

        [ForeignKey(nameof(Producer))]
        public string ProducerID { get; set; }

        public List<Message> Messages { get; set; }
        public Chat()
        {
            Messages = new(); 
        }

    }
}
