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
                Model_id= _configuration["AiKey:Model_chat"]!,

                System_prompt = """
                You are a Fashion Design State Generator.

                You MUST return a valid JSON object matching EXACTLY the following schema.

                Do not add extra properties.
                Do not omit any property.
                Unknown values must be empty strings.
                Lists must always exist.
                Return JSON only.

                Schema:

                {
                  
                    "type": "",
                    "fit": "",
                    "sleeve": "",
                    "length": "",
                    "colorprimary": "",
                    "secondary": "",
                    "material": "",
                    "texture": "",
                    "theme": "",
                    "details": [],
                    "views": "",
                    "background": ""
                  
                }
                """,

                Messages = new List<chatMessage>
                {
                    new chatMessage { role = "user", content = userPrompt }
                },
                Max_tokens=500

            };

            var json = JsonSerializer.Serialize(itiRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string _apiKey = _configuration["AiKey:ApiKey"]!;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ITI API Raw Response]: {responseString}");

            var itiResponse = JsonSerializer.Deserialize<ChatResponse>(responseString);

            var state = JsonSerializer.Deserialize<DesignState>(
                itiResponse!.output_text,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return state!;
        }

        public async Task<DesignState> UpdateDesignStateAsync(DesignState currentState,string userRequest)
        {
            var url = _configuration["AiKey:EndPoint_chat"]!;


            var itiRequest = new ItiChatRequest
            {
                Model_id = _configuration["AiKey:Model_chat"]!,

                System_prompt = $"""
                You are a Fashion Design State Updater.

                Rules:
                - Return ONLY valid JSON.
                - Do not use markdown.
                - Do not add properties.
                - Do not remove properties.
                - Keep existing values unless the user explicitly changes them.
                - Update the Design State according to the user's request.
                """,

                Messages = new List<chatMessage>
                {
                    new chatMessage { role = "user",
                        content = $"""
                        Current Design State:

                        {JsonSerializer.Serialize(currentState)}

                        User Request:

                        {userRequest}
                        """ }
                },
                Max_tokens = 750

            };
            var json = JsonSerializer.Serialize(itiRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            string _apiKey = _configuration["AiKey:ApiKey"]!;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API error: {response.StatusCode} - {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[ITI API Raw Response]: {responseString}");

            var itiResponse = JsonSerializer.Deserialize<ChatResponse>(responseString);

            var state = JsonSerializer.Deserialize<DesignState>(
                itiResponse!.output_text,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return state!;

        }
    }
}
