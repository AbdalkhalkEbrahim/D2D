namespace Domain.DTOs
{
    public class IdentityValidationRequest
    {
        public required string FrontImageUrl { get; set; }
        public required string BackImageUrl { get; set; }
        public required string SelfieImageUrl { get; set; }
    }
}
