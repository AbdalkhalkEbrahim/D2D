namespace V02.DTOs
{
    public class GenerateDesignRequestdto
    {
        public string UserId { get; set; } 
        public Guid? DesignId { get; set; } 
        public string UserMessage { get; set; } = string.Empty;
    }
}
