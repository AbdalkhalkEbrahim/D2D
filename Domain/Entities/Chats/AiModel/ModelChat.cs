using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Chats.AiModel
{
    public class ModelChat
    {
        public int ID { get; set; }
        public int MaxChatTokens { get; set; } = 3;

        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public virtual ICollection<ModelGeneratedDesign> Designs { get; set; }
        
        public ModelChat()
        {
            Designs = new List<ModelGeneratedDesign>();
        }
    }
}
