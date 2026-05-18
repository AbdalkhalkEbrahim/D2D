using Domain.Entities.Customers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Chats
{
    public class Chat : Audits
    {
        public int ID { get; set; }
        public string Name { get;  set; }
        public virtual Producer Producer { get; set; }
        public virtual Customer Customer { get; set; }
        public int ProdicerLimit { get; private set; } = 100;
        public int CustomerLimit { get; private set; } = 50;
        public int ProducerCount { get; set; }
        public int CustomerCount { get; set; }
       
        [ForeignKey(nameof(Customer))]
        public Guid CustomerID { get; set; }

        [ForeignKey(nameof(Producer))]
        public Guid ProducerID { get; set; }

        public List<Message> Messages { get; set; }
        public Chat()
        {
            Messages = new(); 
        }

    }
}
