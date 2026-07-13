using Domain.Entities.Chats.AiModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
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
            // 1. جلب الرابط الأساسي لـ Pollinations (مثال: https://image.pollinations.ai/p/)
            var baseUrl = _configuration["AiKey:EndPoint_gene"]!;
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            // 2. تنظيف الـ Prompt تماماً من أي رموز خاصة قد تكسر الـ URL وتسبب خطأ 404
            string cleanPrompt = prompt
                .Replace(".", " ")
                .Replace(",", " ")
                .Replace("?", " ")
                .Replace("(", " ")
                .Replace(")", " ")
                .Replace("\"", " ")
                .Replace("'", " ")
                .Trim();

            // 3. تشفير النص ليكون صالحاً للعبور بأمان داخل الـ URL
            string sanitizedPrompt = Uri.EscapeDataString(cleanPrompt);

            // 4. بناء رابط الـ GET المباشر واستهداف محرك FLUX مع الأبعاد و Seed عشوائي متجدد
            string fluxUrl = $"{baseUrl}{sanitizedPrompt}?width=1024&height=1024&model=flux&seed={new Random().Next(1, 999999)}";

            // 5. 🔥 الاحترافية هنا: إذا كان هناك صورة سابقة مخزنة في قاعدة البيانات وتم تمريرها في الـ State
            // نقوم بحقن رابطها داخل الـ URL لكي يقوم FLUX بعمل تعديل Image-to-Image بناءً على الهيكل القديم
            if (state != null && !string.IsNullOrEmpty(state.OriginalImageReference))
            {
                string encodedImageRef = Uri.EscapeDataString(state.OriginalImageReference);
                fluxUrl += $"&image={encodedImageRef}";
            }

            // 6. إرجاع الرابط فوراً للـ Controller ومنه للفرونت إند (أداء فوري بدون تعليق للسيرفر)
            return await Task.FromResult(fluxUrl);
        }

        public Task<string> GenerateImageAsync(string enhancedPrompt)
        {
            return GenerateOrEditImageAsync(enhancedPrompt, new DesignState());
        }
    }
}