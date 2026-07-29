using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class IdentityVerificationResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("nationalIdMatched")]
        public bool NationalIdMatched { get; set; }

        [JsonPropertyName("nationalId")]
        public string? NationalId { get; set; }

        [JsonPropertyName("frontNationalId")]
        public string? FrontNationalId { get; set; }

        [JsonPropertyName("backNationalId")]
        public string? BackNationalId { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("reason")]
        public string? Reason { get; set; }

        [JsonPropertyName("verificationReport")]
        public VerificationReport? VerificationReport { get; set; }
    }
}
