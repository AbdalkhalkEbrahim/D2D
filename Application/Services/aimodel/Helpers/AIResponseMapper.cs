using System.Text.Json;

namespace V02
{
    public static class AIResponseMapper
    {
        public static T Map<T>(string response)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(
                    response,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })!;
            }
            catch
            {
                throw new Exception("Failed to parse AI response");
            }
        }
    }
}
