using Domain.Entities.Customers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Chats.AiModel
{
    public class ModelChat
    {
        public int ID { get; set; }
        public  string Title { get; set; }
        public virtual  Customer Customer { get; set; }
        public virtual ICollection<ModelChatMessage> Message { get; set; }
        [ForeignKey(nameof(Customer))]
        public  Guid CustomerID { get; set; }
        public ModelChat()
        {
            Message = new List<ModelChatMessage>();
        }
    }
}
