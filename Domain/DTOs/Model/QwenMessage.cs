using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class QwenMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "user";

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("images")]
        public List<QwenImageItem> Images { get; set; } = new();
    }
}
