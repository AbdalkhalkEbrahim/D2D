
namespace Domain.Entities.Chats.AiModel
{
    public class DesignState
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Fit { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;
        public string Sleeve { get; set; } = string.Empty;
        public string Gender { get; set; } = "Unisex";
        public string PrimaryColor { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string Texture { get; set; } = string.Empty;
        public string Theme { get; set; } = string.Empty;
        public List<string> Details { get; set; } = new();
        public string View { get; set; } = "Front and Back";
        public string Background { get; set; } = "White Studio";

        // Foreign key
    }
}
