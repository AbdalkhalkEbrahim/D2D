using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class ItiChatTextResponse
    {
        [JsonPropertyName("output_text")]
        public string OutputText { get; set; }
    }
}
