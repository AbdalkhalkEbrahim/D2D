using Domain.Entities.Chats.AiModel;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using V02.DTOs;

namespace V02.Services
{
    public class ModelChatService : IModelChatService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ModelChatService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<DesignState> EnhancePromptAsync(string userPrompt)
        {
            var url = _configuration["AiKey:EndPoint_chat"]!;

            var itiRequest = new ItiChatRequest
            {
                Model_id = _configuration["AiKey:Model_chat"]!, // anthropic.claude-3-5-sonnet-v2:0
                System_prompt = """
                You are an expert Fashion Design State Generator driven by Claude Sonnet.
                You MUST return a valid, pure JSON object matching EXACTLY the following schema.
                Do not wrap it in markdown block quotes like ```json. Return raw JSON string only.

                Schema:
                {
                    "type": "",
                    "fit": "",
                    "sleeve": "",
                    "length": "",
                    "neckline": "",
                    "styleCategory": "",
                    "targetGender": "",
                    "colorPrimary": "",
                    "secondary": "",
                    "material": "",
                    "fabricWeight": "",
                    "texture": "",
                    "theme": "",
                    "details": [],
                    "views": "photorealistic fashion studio showcase view",
                    "background": "clean solid minimalist background",
                    "modificationArea": "",
                    "inpaintMaskPrompt": "",
                    "originalImageReference": ""
                }
                """,
                Messages = new List<chatMessage>
                {
                    new chatMessage { role = "user", content = userPrompt }
                },
                Max_tokens = 3000
            };

            return await ExecuteChatApiAsync(url, itiRequest);
        }

        public async Task<DesignState> UpdateDesignStateAsync(DesignState currentState, string userRequest)
        {
            var url = _configuration["AiKey:EndPoint_chat"]!;

            var itiRequest = new ItiChatRequest
            {
                Model_id = _configuration["AiKey:Model_chat"]!,
                System_prompt = """
                You are a Fashion Design State Updater driven by Claude Sonnet.
                Compare the user's request with the current state.
                CRITICAL RULE: If the user wants to EDIT, REPLACE, or CHANGE a specific part of the existing design (e.g., 'change sleeves to leather', 'replace the collar to v-neck', 'change color of pockets'), you MUST:
                1. Fill the "modificationArea" with the garment part name.
                2. Fill the "inpaintMaskPrompt" with the exact descriptive word for that specific part to isolate it (e.g., "sleeves", "collar", "pockets").
                Return ONLY the updated valid JSON matching the exact schema structure. No extra words.
                """,
                Messages = new List<chatMessage>
                {
                    new chatMessage
                    {
                        role = "user",
                        content = $"Current State:\n{JsonSerializer.Serialize(currentState)}\n\nUser Request: {userRequest}"
                    }
                },
                Max_tokens = 3000
            };

            return await ExecuteChatApiAsync(url, itiRequest);
        }

        private async Task<DesignState> ExecuteChatApiAsync(string url, ItiChatRequest itiRequest)
        {
            var json = JsonSerializer.Serialize(itiRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string apiKey = _configuration["AiKey:ApiKey"]!;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Claude API Error: {response.StatusCode} - {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var itiResponse = JsonSerializer.Deserialize<ChatResponse>(responseString);

            var cleanJson = itiResponse!.output_text.Replace("```json", "").Replace("```", "").Trim();

            var state = JsonSerializer.Deserialize<DesignState>(cleanJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return state!;
        }
    }
}