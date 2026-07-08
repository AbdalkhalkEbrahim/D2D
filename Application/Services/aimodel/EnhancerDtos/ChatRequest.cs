using System.Text.Json.Serialization;

namespace V02.DTOs
{
    public class ItiChatRequest
    {
        [JsonPropertyName("model_id")]
        public string Model_id { get; set; }=string.Empty;

        [JsonPropertyName("messages")]
        public List<chatMessage> Messages { get; set; } = [];

        [JsonPropertyName("system_prompt")]
        public string System_prompt { get; set; } = "You are an expert fashion prompt engineer.";

        [JsonPropertyName("max_tokens")]
        public int Max_tokens { get; set; } = 300;
    }
}
