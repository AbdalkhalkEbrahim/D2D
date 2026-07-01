namespace Domain.DTOs.ModelDtos
{
    public class IdentityValidationResponse
    {
        public int SimilarityScore { get; set; }

        public string DocumentQuality { get; set; }

        public string Notes { get; set; }

        public bool NeedsManualReview { get; set; }
    }
}