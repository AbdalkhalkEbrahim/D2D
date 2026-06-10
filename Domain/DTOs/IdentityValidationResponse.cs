namespace Domain.DTOs
{
    public class IdentityValidationResponse
    {
        public float? SimilarityScore { get; set; }

        public string? DocumentQuality { get; set; }

        public string? Notes { get; set; }

        public bool NeedsManualReview { get; set; }
    }
}