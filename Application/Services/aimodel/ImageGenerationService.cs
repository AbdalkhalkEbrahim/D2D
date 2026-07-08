using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace V02.Services
{


    public class ImageGenerationService : IImageGenerationService
    {
        private readonly HttpClient _httpClient;

        public ImageGenerationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Endpoint of Hugging Face for model "Flux Dev"
        public async Task<string> GenerateImageAsync(string enhancedPrompt)
        {
            try
            {

                string encodedPrompt = HttpUtility.UrlEncode(enhancedPrompt);

                string imageUrl = $"https://image.pollinations.ai/p/{encodedPrompt}?width=1024&height=1024&model=flux&nologo=true";

                byte[] imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);

                string base64Image = Convert.ToBase64String(imageBytes);

                return $"data:image/jpeg;base64,{base64Image}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error : {ex.Message}");
            }
        }
    }
}