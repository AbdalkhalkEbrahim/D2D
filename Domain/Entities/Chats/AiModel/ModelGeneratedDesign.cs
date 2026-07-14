using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Chats.AiModel
{
    public class ModelGeneratedDesign
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Customer Customer { get; set; }
        public string ImageUrl { get; set; } = string.Empty; 
        public string PromptUsed { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdatedAt { get; set; }
    }
}
