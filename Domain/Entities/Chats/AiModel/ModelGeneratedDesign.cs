using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Chats.AiModel
{
    public class ModelGeneratedDesign
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        [ForeignKey(nameof(Customer))]
        public string CustomerId { get; set; }

        public List<Image> Images { get; set; } = new();
        public DesignState? DesignState { get; set; }
        [ForeignKey("DesignState")]
        public Guid DesignStateId { get; set; }
    }
}
