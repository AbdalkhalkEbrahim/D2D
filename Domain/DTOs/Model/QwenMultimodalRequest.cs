using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class QwenMultimodalRequest
    {
        [JsonPropertyName("model_id")]
        public string ModelId { get; set; }

        [JsonPropertyName("messages")]
        public List<QwenMessage> Messages { get; set; } = new();
    }
}
