using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Chats.AiModel
{
    public class ModelChat
    {
        public int ID { get; set; }
        public  string Title { get; set; }
        public virtual  Customer Customer { get; set; }
        public virtual ICollection<ModelChatMessage> Message { get; set; }
        [ForeignKey(nameof(Customer))]
        public  string CustomerID { get; set; }
        public ModelChat()
        {
            Message = new List<ModelChatMessage>();
        }
    }
}
