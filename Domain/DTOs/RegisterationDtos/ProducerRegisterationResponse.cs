using Domain.Entities.Producers;
using Domain.Enums.Status;
using System.Text.Json.Serialization;

namespace Domain.DTOs.RegisterationDtos
{
    public class ProducerRegisterationResponse
    {
        public required string UserId { get; set; }

        public required string FrontImageID { get; set; }
        public required string BackImageID { get; set; }
        public required string PersonalImage { get; set; }
        public ICollection<LicenseVerification> LicenseVerification { get; set; } = new List<LicenseVerification>();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public VerificationStatus VerificationStatus { get; set; }
        public float? SimilarityScore { get; set; }
        public string? DocumentQuality { get; set; }
        public string? Notes { get; set; }
        public bool NeedsManualReview { get; set; }
    }
}
