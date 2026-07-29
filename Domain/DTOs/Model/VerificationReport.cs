using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class VerificationReport
    {
        [JsonPropertyName("samePerson")]
        public bool SamePerson { get; set; }

        [JsonPropertyName("faceSimilarity")]
        public decimal FaceSimilarity { get; set; }

        [JsonPropertyName("confidence")]
        public string? Confidence { get; set; }

        [JsonPropertyName("decision")]
        public string? Decision { get; set; }

        [JsonPropertyName("faceVisibility")]
        public string? FaceVisibility { get; set; }

        [JsonPropertyName("idImageQuality")]
        public string? IdImageQuality { get; set; }

        [JsonPropertyName("selfieImageQuality")]
        public string? SelfieImageQuality { get; set; }

        [JsonPropertyName("blurDetected")]
        public bool BlurDetected { get; set; }

        [JsonPropertyName("glareDetected")]
        public bool GlareDetected { get; set; }

        [JsonPropertyName("occlusionDetected")]
        public bool OcclusionDetected { get; set; }

        [JsonPropertyName("faceOrientation")]
        public string? FaceOrientation { get; set; }

        [JsonPropertyName("lightingQuality")]
        public string? LightingQuality { get; set; }

        [JsonPropertyName("possibleIssues")]
        public List<string>? PossibleIssues { get; set; }

        [JsonPropertyName("observations")]
        public List<string>? Observations { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }
    }
}
