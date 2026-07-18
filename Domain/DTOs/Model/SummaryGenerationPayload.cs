using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class SummaryGenerationPayload
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("isSimpleImagination")]
        public bool IsSimpleImagination { get; set; }
        public int ChatId { get; set; }
    }
}
