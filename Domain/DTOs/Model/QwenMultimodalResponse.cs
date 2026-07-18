using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class QwenMultimodalResponse
    {
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        [JsonPropertyName("model_id")]
        public string ModelId { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }

        [JsonPropertyName("output_text")]
        public string OutputText { get; set; }


        [JsonPropertyName("usage")]
        public QwenUsage Usage { get; set; }

        [JsonPropertyName("estimated_cost_usd")]
        public string EstimatedCostUsd { get; set; }

        [JsonPropertyName("actual_cost_usd")]
        public string ActualCostUsd { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
