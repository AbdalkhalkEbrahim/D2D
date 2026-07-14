using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities.Chats.AiModel;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace V02.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatModelController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;

        private const string SystemGenerationPrompt =
            "Describe this image in detailes, seperate the description into 2 section, " +
            "one for the design front and the other fot the design back as when passing " +
            "this description to another model it can regenrate a very similar design to it. " +
            "descripe the shapes or axs of anything in the image.just a plain text without " +
            "any characters focus on the design itself with colors' degree, logos, lines " +
            "and how it looks and the positions of everything the size and thickness of " +
            "any printed logo or lines or drawings, descriping how it looks detailly";

        public ChatModelController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUploadService uploadService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _uploadService = uploadService;
        }

        [HttpPost("analyze-design")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AnalyzeDesign(IFormFile? file, [FromForm] string? additionalText)
        {
            bool hasImage = file != null && file.Length > 0;
            bool hasText = !string.IsNullOrWhiteSpace(additionalText);

            if (!hasImage && !hasText)
            {
                return BadRequest("You must provide either an image, a text description, or both.");
            }

            if (hasText && !hasImage)
            {
                return Ok(additionalText.Trim());
            }

            try
            {
                string rawBase64;
                using (var ms = new MemoryStream())
                {
                    await file!.CopyToAsync(ms);
                    byte[] fileBytes = ms.ToArray();
                    rawBase64 = Convert.ToBase64String(fileBytes);
                }

                string dynamicPrompt = SystemGenerationPrompt;

                if (hasText)
                {
                    dynamicPrompt += "\n\nCRITICAL MODIFICATION REQUEST:\n" +
                                     "The user wants to apply changes to the design shown in the image. " +
                                     "Incorporate the following modifications directly into your final front/back description " +
                                     $"so it describes the updated version: {additionalText.Trim()}";
                }

                var requestBody = new QwenMultimodalRequest
                {
                    ModelId = _configuration["AiKey:Model_description_image"] ?? "qwen.qwen3-vl-235b-a22b",
                    Messages = new List<QwenMessage>
                    {
                        new QwenMessage
                        {
                            Role = "user",
                            Text = dynamicPrompt,
                            Images = new List<QwenImageItem>
                            {
                                new QwenImageItem
                                {
                                    DataBase64 = rawBase64,
                                    Type = file.ContentType
                                }
                            }
                        }
                    }
                };

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AiKey:ApiKey"]);

                string serialPayload = JsonSerializer.Serialize(requestBody);
                var contentToSend = new StringContent(serialPayload, Encoding.UTF8, "application/json");

                string endpoint = _configuration["AiKey:EndPoint_description_image"] ?? "http://apiaccess.iti.net.eg/api/v1/student/multimodal-chat";
                HttpResponseMessage response = await client.PostAsync(endpoint, contentToSend);

                if (!response.IsSuccessStatusCode)
                {
                    string failedBody = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Upstream Service Failure: {failedBody}");
                }

                string jsonResponseString = await response.Content.ReadAsStringAsync();
                var itiResult = JsonSerializer.Deserialize<QwenMultimodalResponse>(jsonResponseString);

                if (itiResult == null || string.IsNullOrEmpty(itiResult.OutputText))
                {
                    return StatusCode(502, "The upstream model returned an unexpected or empty response body.");
                }

                return Ok(itiResult.OutputText);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during multi-case design execution: {ex.Message}");
            }
        }

        [HttpPost("generate-summaries")]
        public async Task<IActionResult> GenerateSummaries([FromBody] SummaryGenerationPayload payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.Description))
            {
                return BadRequest("The description content cannot be empty.");
            }

            try
            {
                string structuralSystemInstructions = string.Empty;

                if (payload.IsSimpleImagination)
                {
                    structuralSystemInstructions =
                        "You am a creative design assistant. The user has provided a simple, rough concept for a design. " +
                        "Generate exactly 5 different apparel prompt variants based on this concept. " +
                        "CRITICAL INSTRUCTIONS FOR VARIATION:\n" +
                        $"1. The first variant '[SUM_1]' must be a polished, direct reflection of the user's exact raw input text: \"{payload.Description.Trim()}\".\n" +
                        "2. Variants '[SUM_2]', '[SUM_3]', '[SUM_4]', and '[SUM_5]' must introduce distinct, artistic design variations (e.g., alter the logo placements, try different graphic styles, shift line layouts or thicknesses, shuffle visual positions) while keeping the overarching product identity recognizable. This provides a diverse stylistic choice catalog.\n" +
                        "CRITICAL COMPOSITION MANDATE FOR EVERY VARIANT: You must explicitly state that the final output image displays both the front and back views of the t-shirt side-by-side in a single layout. The garment must be hung neatly on a hanger, perfectly centered in the middle of the frame, fully visible, and captured with clear, professional catalog clarity.\n" +
                        "CRITICAL FORMATTING: You must separate each variant using exact delimiters '[SUM_1]', '[SUM_2]', '[SUM_3]', '[SUM_4]', '[SUM_5]' followed immediately by the text. Do not include any other markdown formatting, headers, conversational intro, or outro text.";
                }
                else
                {
                    structuralSystemInstructions =
                        "You are a precise design assistant. Below is a detailed description of an apparel design. " +
                        "Generate exactly 5 different summaries of this design. " +
                        "Each summary must be a concise version keeping all essential design landmarks, with an overall structural similarity between summaries of no less than 85% to ensure design consistency.\n" +
                        "CRITICAL COMPOSITION MANDATE FOR EVERY SUMMARY: You must explicitly state that the final output image displays both the front and back views of the t-shirt side-by-side in a single layout. The garment must be hung neatly on a hanger, perfectly centered in the middle of the frame, fully visible, and captured with clear, professional catalog clarity.\n" +
                        "CRITICAL FORMATTING: You must separate each summary using exact delimiters '[SUM_1]', '[SUM_2]', '[SUM_3]', '[SUM_4]', '[SUM_5]' followed immediately by the text. Do not include any other markdown formatting, headers, conversational intro, or outro text outside these blocks.\n\n" +
                        $"Detailed Description:\n{payload.Description.Trim()}";
                }

                var requestBody = new ItiChatTextRequest
                {
                    ModelId = _configuration["AiKey:Model_summarizer"] ?? "openai.gpt-oss-120b-1:0",
                    Messages = new List<ItiTextMessage>
                    {
                        new ItiTextMessage
                        {
                            Role = "user",
                            Content = structuralSystemInstructions
                        }
                    }
                };

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AiKey:ApiKey"]);

                string serialPayload = JsonSerializer.Serialize(requestBody);
                var contentToSend = new StringContent(serialPayload, Encoding.UTF8, "application/json");

                string endpoint = _configuration["AiKey:EndPoint_summarizer"] ?? "http://apiaccess.iti.net.eg/api/v1/student/chat";
                HttpResponseMessage response = await client.PostAsync(endpoint, contentToSend);

                if (!response.IsSuccessStatusCode)
                {
                    string failedBody = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Text Upstream Service Failure: {failedBody}");
                }

                string jsonResponseString = await response.Content.ReadAsStringAsync();
                var textResult = JsonSerializer.Deserialize<ItiChatTextResponse>(jsonResponseString);

                if (textResult == null || string.IsNullOrEmpty(textResult.OutputText))
                {
                    return StatusCode(502, "The upstream model returned an empty text generation field.");
                }

                string rawModelOutput = textResult.OutputText;
                var summariesList = new List<string>();
                string[] tags = { "\\[SUM_1\\]", "\\[SUM_2\\]", "\\[SUM_3\\]", "\\[SUM_4\\]", "\\[SUM_5\\]" };

                string pattern = string.Join("|", tags);
                string[] rawParts = Regex.Split(rawModelOutput, pattern);

                foreach (var part in rawParts)
                {
                    string cleanPart = part.Trim();
                    if (!string.IsNullOrEmpty(cleanPart))
                    {
                        summariesList.Add(cleanPart);
                    }
                }

                if (summariesList.Count < 5)
                {
                    summariesList.Clear();
                    string[] alternativeParts = Regex.Split(rawModelOutput, @"summarize\d+\s*:\s*", RegexOptions.IgnoreCase);
                    foreach (var part in alternativeParts)
                    {
                        string cleanPart = part.Trim();
                        if (!string.IsNullOrEmpty(cleanPart)) summariesList.Add(cleanPart);
                    }
                }

                if (payload.IsSimpleImagination)
                    summariesList.Add(payload.Description.Trim());

                return Ok(summariesList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during summary variations processing: {ex.Message}");
            }
        }

        [HttpPost("generate-design-image")]
        public async Task<IActionResult> GenerateDesignImage([FromBody] string summaryDescription, string userId)
        {
            if (string.IsNullOrWhiteSpace(summaryDescription))
            {
                return BadRequest("The design summary description prompt cannot be blank.");
            }

            try
            {
                var requestPayload = new FluxGenerationRequest
                {
                    Inputs = summaryDescription
                };

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["AiKey:hugingface_key"]);

                string jsonString = JsonSerializer.Serialize(requestPayload);
                var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

                string endpoint = _configuration["AiKey:EndPoint_generate_image"];
                HttpResponseMessage response = await client.PostAsync(endpoint, stringContent);

                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, $"Hugging Face Inference Error: {errorContent}");
                }

                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                string contentType = response.Content.Headers.ContentType?.MediaType ?? "image/png";

                var stream = new MemoryStream(imageBytes);
                IFormFile fileToUpload = new FormFile(stream, 0, imageBytes.Length, "file", "generated_design.png")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = contentType
                };
                var modelGeneratedDesign = new ModelGeneratedDesign
                {
                    UserId = userId,
                    PromptUsed = summaryDescription,
                    CreatedAt = DateTime.UtcNow,

                };
                var file = await _uploadService.ChangeFileFormat(new List<IFormFile> { fileToUpload });
                BackgroundJob.Enqueue<IUploadService>(service => service.UploadAndSaveSingleFile(modelGeneratedDesign, "ImageUrl", file[0],true));
                return File(imageBytes, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during design image generation processing: {ex.Message}");
            }
        }

        public class QwenMultimodalRequest
        {
            [JsonPropertyName("model_id")]
            public string ModelId { get; set; }

            [JsonPropertyName("messages")]
            public List<QwenMessage> Messages { get; set; } = new();
        }

        public class QwenMessage
        {
            [JsonPropertyName("role")]
            public string Role { get; set; } = "user";

            [JsonPropertyName("text")]
            public string Text { get; set; }

            [JsonPropertyName("images")]
            public List<QwenImageItem> Images { get; set; } = new();
        }

        public class QwenImageItem
        {
            [JsonPropertyName("data_base64")]
            public string DataBase64 { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }
        }

        public class QwenMultimodalResponse
        {
            [JsonPropertyName("request_id")]
            public string RequestId { get; set; }

            [JsonPropertyName("model_id")]
            public string ModelId { get; set; }

            [JsonPropertyName("region")]
            public string Region { get; set; }

            [JsonPropertyName("output_text")]
            public string OutputText { get; set; }

            [JsonPropertyName("usage")]
            public QwenUsage Usage { get; set; }

            [JsonPropertyName("estimated_cost_usd")]
            public string EstimatedCostUsd { get; set; }

            [JsonPropertyName("actual_cost_usd")]
            public string ActualCostUsd { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }
        }

        public class QwenUsage
        {
            [JsonPropertyName("input_tokens")]
            public int InputTokens { get; set; }

            [JsonPropertyName("output_tokens")]
            public int OutputTokens { get; set; }

            [JsonPropertyName("total_tokens")]
            public int TotalTokens { get; set; }

            [JsonPropertyName("stop_reason")]
            public string StopReason { get; set; }

            [JsonPropertyName("budget_state")]
            public string BudgetState { get; set; }

            [JsonPropertyName("fallback_used")]
            public bool FallbackUsed { get; set; }
        }

        public class ItiChatTextRequest
        {
            [JsonPropertyName("model_id")]
            public string ModelId { get; set; }

            [JsonPropertyName("messages")]
            public List<ItiTextMessage> Messages { get; set; } = new();
        }

        public class ItiTextMessage
        {
            [JsonPropertyName("role")]
            public string Role { get; set; } = "user";

            [JsonPropertyName("content")]
            public string Content { get; set; }
        }

        public class ItiChatTextResponse
        {
            [JsonPropertyName("output_text")]
            public string OutputText { get; set; }
        }

        public class FluxGenerationRequest
        {
            [JsonPropertyName("inputs")]
            public string Inputs { get; set; }
        }

        public class SummaryGenerationPayload
        {
            [JsonPropertyName("description")]
            public string Description { get; set; } = string.Empty;

            [JsonPropertyName("isSimpleImagination")]
            public bool IsSimpleImagination { get; set; }
        }
    }
}