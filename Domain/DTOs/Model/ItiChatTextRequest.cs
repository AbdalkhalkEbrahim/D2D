using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class ItiChatTextRequest
    {
        [JsonPropertyName("model_id")]
        public string ModelId { get; set; }

        [JsonPropertyName("messages")]
        public List<ItiTextMessage> Messages { get; set; } = new();
    }
}
