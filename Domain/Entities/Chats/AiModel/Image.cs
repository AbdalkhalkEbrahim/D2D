using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Chats.AiModel
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Base64String { get; set; } = string.Empty;

        // Foreign key
        [ForeignKey(nameof(Design))]
        public Guid DesignId { get; set; }
        public ModelGeneratedDesign? Design { get; set; }
    }
}
