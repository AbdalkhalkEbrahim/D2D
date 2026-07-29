using Domain.Entities.Customers;
using Domain.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Chats.AiModel
{
    public class ModelGeneratedDesign:Audits
    {
        public Guid Id { get; set; }
        public ModelChat ModelChat { get; set; }
        [ForeignKey(nameof(ModelChat))]
        public int ModelChatId { get; set; }
        public string? ImageUrl { get; set; } 
        public string PromptUsed { get; set; } 
    }
}
