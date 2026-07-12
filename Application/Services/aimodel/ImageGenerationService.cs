using Domain.Entities.Chats.AiModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using V02.DTOs;

namespace V02.Services
{
    public class ImageGenerationService : IImageGenerationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ImageGenerationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GenerateOrEditImageAsync(string prompt, DesignState state)
        {
            // 🛠️ 1. مسار الـ Inpainting (تعديل جزء من الصورة عبر جيتواي المعهد)
            if (!string.IsNullOrEmpty(state.OriginalImageReference) && !string.IsNullOrEmpty(state.InpaintMaskPrompt))
            {
                string apiKey = _configuration["AiKey:ApiKey"]!;
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var urlInpaint = _configuration["AiKey:EndPoint_identity"]!;

                var inpaintRequest = new
                {
                    model_id = _configuration["AiKey:Model_inpaint"]!,
                    prompt = prompt,
                    mask_prompt = state.InpaintMaskPrompt,
                    image_url = state.OriginalImageReference
                };

                var content = new StringContent(JsonSerializer.Serialize(inpaintRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(urlInpaint, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"Stable Inpaint API Failure: {await response.Content.ReadAsStringAsync()}");

                var resString = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(resString);
                string base64 = doc.RootElement.GetProperty("output_image_base64").GetString()!;
                return $"data:image/png;base64,{base64}";
            }
            // 🎨 2. مسار الـ Generation الحر (إنشاء تصميم جديد كلياً باستخدام FLUX.1-schnell)
            else
            {
                var baseUrl = "https://image.pollinations.ai/p/";

                // 1. تنظيف الـ Prompt تماماً لضمان سلامة الـ URL
                string cleanPrompt = prompt
                    .Replace(".", " ")
                    .Replace(",", " ")
                    .Replace("?", " ")
                    .Replace("(", " ")
                    .Replace(")", " ")
                    .Replace("\"", " ")
                    .Replace("'", " ")
                    .Trim();

                string sanitizedPrompt = Uri.EscapeDataString(cleanPrompt);
                string fluxUrl = $"{baseUrl}{sanitizedPrompt}?width=1024&height=1024&model=flux&seed={new Random().Next(1, 999999)}";

                // 2. ضبط حد أقصى لانتظار الطلب (Timeout) لمنع التعليق اللانهائي
                // إذا لم يستجب السيرفر خلال 45 ثانية سيقطع الاتصال ويرمي خطأ واضح بدلاً من التعليق
                _httpClient.Timeout = TimeSpan.FromSeconds(45);

                var requestMessage = new HttpRequestMessage(HttpMethod.Get, fluxUrl);
                requestMessage.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                // 3. استخدام HttpCompletionOption.ResponseHeadersRead لمنع الـ Deadlock
                // هذا الخيار يخبر دوت نت: "افتح المسار فوراً بمجرد استلام الهيدرز، ولا تنتظر تحميل الصورة بالكامل في الـ Memory"
                var response = await _httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContext = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Flux GET Failure: {response.StatusCode} - {errorContext}");
                }

                // 4. قراءة البيانات على هيئة Stream وتحويلها لـ MemoryStream لتجنب تعليق السيرفر
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                using (var ms = new System.IO.MemoryStream())
                {
                    await responseStream.CopyToAsync(ms);
                    byte[] imageBytes = ms.ToArray();

                    string base64Image = Convert.ToBase64String(imageBytes);
                    return $"data:image/png;base64,{base64Image}";
                }
            }
        }

        public Task<string> GenerateImageAsync(string enhancedPrompt)
        {
            return GenerateOrEditImageAsync(enhancedPrompt, new DesignState());
        }
    }
}