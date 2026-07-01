using Application.Interfaces;
using Application.Response;
using Domain.DTOs.ModelDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
namespace Application.Services
{

    public class IdentityValidationService : IIdentityValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly D2DContext _context;

        public IdentityValidationService(HttpClient httpClient, IConfiguration configuration, D2DContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }
        public async Task ValidateAndApproveUserIdentityAsync(string userId, string idFrontBase64, string idBackBase64, string selfieBase64)
        {
            var aiResult = await AnalyzeAsync(idFrontBase64, idBackBase64, selfieBase64);

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                if (aiResult.SimilarityScore >= 75 && !aiResult.NeedsManualReview)
                {
                    user.IdentityStatus = VerificationStatus.Approved;
                }
                else
                {
                    user.IdentityStatus = VerificationStatus.Rejected;
                }
                Console.WriteLine("===============================================");
                Console.WriteLine($"Similarity scroe = {aiResult.SimilarityScore}\n Notes = {aiResult.Notes}\n  DocumentQuality = {aiResult.DocumentQuality}\n  NeedsManualReview = {aiResult.NeedsManualReview}\n");
                Console.WriteLine("===============================================");

                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IdentityValidationResponse> AnalyzeAsync(string idFront, string idBack, string selfie)
        {
            try
            {
                var url = _configuration["AiKey:EndPoint_identity"]!;
                string apiKey = _configuration["AiKey:ApiKey"]!;

                if (!string.IsNullOrEmpty(apiKey))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                }

                string cleanFront = CleanBase64(idFront);
                string cleanBack = CleanBase64(idBack);
                string cleanSelfie = CleanBase64(selfie);

                string systemPrompt = """
                You are an advanced Biometric and Identity Verification AI. Your job is to strictly audit 3 images passed in the array: Image 0 (ID Front), Image 1 (ID Back), and Image 2 (Selfie).

                Follow this internal logic step-by-step:
                1. Describe Image 0 (ID Front): Is it a male or female? Is there facial hair?
                2. Describe Image 2 (Selfie): Is it a male or female? Is there facial hair?
                3. Compare Facial Structure: Look at bone structure, eyes, and nose. NOTE: A person growing a beard/mustache or changing lighting between the ID photo and the Selfie is NORMAL and does NOT mean they are different people.
                4. Calculate SimilarityScore: 
                   - If completely different genders or totally different people: Score = 0.
                   - If same person but with natural changes (beard, age, weight): Score should be high (75-95) based on eye/nose alignment.

                You MUST return a valid JSON object matching this schema:
                {
                    "SimilarityScore": 0,
                    "DocumentQuality": "Excellent/Good/Poor",
                    "Notes": "Your step-by-step description and matching logic here",
                    "NeedsManualReview": false
                }
                """;

                string userContent = """
                EXECUTE IDENTITY AUDIT NOW:
                - Image index 0 is the ID Front.
                - Image index 1 is the ID Back.
                - Image index 2 is the Live Selfie.
                Analyze them strictly according to your system instructions and compare index 0 with index 2 carefully.
                """;

                var itiRequestBody = new
                {
                    model_id = "qwen.qwen3-vl-235b-a22b",
                    system_prompt = systemPrompt,
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            text = userContent,
                            images = new[]
                            {
                                new { format = "png", data_base64 = cleanFront },
                                new { format = "png", data_base64 = cleanBack },
                                new { format = "png", data_base64 = cleanSelfie }
                            }
                        }
                    },
                    max_tokens = 1000
                };

                var serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                string jsonPayload = JsonSerializer.Serialize(itiRequestBody, serializerOptions);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"ITI Identity API Error: {response.StatusCode} - {errorDetails}");
                }

                var responseString = await response.Content.ReadAsStringAsync();

                using var doc = JsonDocument.Parse(responseString);
                string rawResponse = doc.RootElement.GetProperty("output_text").GetString() ?? "";

                return AIResponseMapper.Map<IdentityValidationResponse>(rawResponse);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error : {ex.Message}");
            }
        }

        private string CleanBase64(string base64Str)
        {
            if (string.IsNullOrEmpty(base64Str)) return "";

            if (base64Str.Contains(","))
            {
                base64Str = base64Str.Split(',')[1];
            }

            return base64Str.Replace("\r", "").Replace("\n", "").Trim();
        }
    }
}
