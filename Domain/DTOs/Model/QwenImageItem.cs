using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.DTOs.Model
{
    public class QwenImageItem
    {
        [JsonPropertyName("data_base64")]
        public string DataBase64 { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
