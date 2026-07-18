using Application.Interfaces;
using Application.Response;
using Domain.DTOs.Model;
using Infrastructure.Data.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Extensions.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Entities.Chats.AiModel;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using System.Globalization;

namespace Application.Services
{
    public class ModelsService:IModelesService
    {
        private readonly D2DContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;
        public ModelsService(D2DContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration, IUploadService uploadService)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _uploadService = uploadService;
        }
        public async Task<Result<decimal>> AnalaysisImageScore(string prompet, IFormFile image)
        {
            string rawBase64;
            using (var ms = new MemoryStream())
            {
                await image!.CopyToAsync(ms);
                byte[] fileBytes = ms.ToArray();
                rawBase64 = Convert.ToBase64String(fileBytes);
            }

            var requestBody = new QwenMultimodalRequest
            {
                ModelId = _configuration["AiKey:Model_description_image"] ?? "qwen.qwen3-vl-235b-a22b",
                Messages = new List<QwenMessage>
        {
            new QwenMessage
            {
                Role = "user",
                Text = prompet,
                Images = new List<QwenImageItem>
                {
                    new QwenImageItem
                    {
                        DataBase64 = rawBase64,
                        Type = image.ContentType
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
                return Result<decimal>.Failure(Messages.ModelSummaryError(failedBody));
            }

            string jsonResponseString = await response.Content.ReadAsStringAsync();
            var itiResult = JsonSerializer.Deserialize<QwenMultimodalResponse>(jsonResponseString);

            if (itiResult == null || string.IsNullOrWhiteSpace(itiResult.OutputText))
            {
                return Result<decimal>.Failure(Messages.BadRequest.WithTarget("NullValue"));
            }

            if (decimal.TryParse(itiResult.OutputText.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal score))
            {
                return Result<decimal>.Success(score);
            }

            return Result<decimal>.Failure(Messages.BadRequest.WithTarget("InvalidScoreFormat"));
        }
        public async Task<Result<List<string>>> GenerateSummaries(SummaryGenerationPayload payload)
        {
            if (payload == null || string.IsNullOrWhiteSpace(payload.Description))
                return Result<List<string>>.Failure(Messages.BadRequest.WithTarget("NullValue"));

            
            string structuralSystemInstructions = string.Empty;

            if (payload.IsSimpleImagination)
            {
                structuralSystemInstructions =
                    "You are an expert fashion design assistant. " +
                    "The user has provided a simple, rough concept for an apparel design. " +
                    "Generate exactly 5 different apparel prompt variants based on this concept.\n\n" +

                    "STYLE SELECTION RULES:\n" +
                    "1. First, identify the garment category from the user's description (e.g., T-shirt, Hoodie, Dress, Jacket, Pants, Skirt, Polo Shirt, Coat, Shirt, Sweater, etc.).\n" +
                    "2. Based on the identified garment category, automatically choose five different fashion styles that are realistic and appropriate for that specific garment.\n" +
                    "3. Do NOT use predetermined styles. Select the most suitable styles according to the garment category.\n" +
                    "4. Every variant must represent one different fashion style.\n" +
                    "5. Explicitly mention the chosen fashion style at the beginning of every generated prompt.\n\n" +

                    "DESIGN RULES:\n" +
                    $"1. The first variant '[SUM_1]' must be a polished version of the user's original concept: \"{payload.Description.Trim()}\".\n" +
                    "2. Variants '[SUM_2]' through '[SUM_5]' must keep the same core concept while adapting it to different fashion styles.\n" +
                    "3. Preserve the garment category, colors, graphics, logos, branding, artwork, and overall design identity.\n" +
                    "4. Only modify style-related characteristics such as silhouette, fit, proportions, stitching, trims, fabric suggestions, texture, finishing details, and styling direction.\n" +
                    "5. Never change the garment into another category.\n\n" +

                    "CRITICAL COMPOSITION:\n" +
                    "Every generated prompt must explicitly state that the final output image shows BOTH the front and back views of the garment side-by-side in one image. " +
                    "The garment must be hanging neatly on a hanger, perfectly centered, fully visible, isolated on a pure white background, and photographed with professional apparel catalog quality.\n\n" +

                    "CRITICAL FORMATTING:\n" +
                    "Return exactly five sections separated ONLY by the following delimiters:\n" +
                    "[SUM_1]\n" +
                    "[SUM_2]\n" +
                    "[SUM_3]\n" +
                    "[SUM_4]\n" +
                    "[SUM_5]\n" +
                    "Do not include markdown, headings, explanations, introductions, or conclusions.";
            }
            else
            {
                structuralSystemInstructions =
                    "You are an expert fashion design assistant. " +
                    "The user has provided a detailed apparel design description. " +
                    "Generate exactly 5 prompt variations while preserving the original design.\n\n" +

                    "STYLE SELECTION RULES:\n" +
                    "1. Identify the garment category from the provided description.\n" +
                    "2. Automatically choose five realistic fashion styles suitable for that garment category.\n" +
                    "3. Do NOT use fixed styles for every request.\n" +
                    "4. Every generated summary must represent a unique fashion style.\n" +
                    "5. Explicitly mention the selected fashion style at the beginning of each summary.\n\n" +

                    "DESIGN RULES:\n" +
                    "1. Preserve at least 85% structural similarity with the original design.\n" +
                    "2. Keep the garment category unchanged.\n" +
                    "3. Preserve the original graphics, logo, colors, branding, artwork, and overall identity.\n" +
                    "4. Modify only style-related elements including silhouette, fit, proportions, stitching, trims, fabric choice, finishing details, texture, and styling direction.\n" +
                    "5. Never convert the garment into another category.\n\n" +

                    "CRITICAL COMPOSITION:\n" +
                    "Every generated prompt must explicitly state that the final output image shows BOTH the front and back views of the garment side-by-side in one image. " +
                    "The garment must be hanging neatly on a hanger, perfectly centered, fully visible, isolated on a pure white background, and photographed with professional apparel catalog quality.\n\n" +

                    $"Original Design:\n{payload.Description.Trim()}\n\n" +

                    "CRITICAL FORMATTING:\n" +
                    "Return exactly five sections separated ONLY by the following delimiters:\n" +
                    "[SUM_1]\n" +
                    "[SUM_2]\n" +
                    "[SUM_3]\n" +
                    "[SUM_4]\n" +
                    "[SUM_5]\n" +
                    "Do not include markdown, headings, explanations, introductions, or conclusions.";
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
                    return Result<List<string>>.Failure(Messages.ModelSummaryError(failedBody));
                }

                string jsonResponseString = await response.Content.ReadAsStringAsync();
                var textResult = JsonSerializer.Deserialize<ItiChatTextResponse>(jsonResponseString);

                if (textResult == null || string.IsNullOrEmpty(textResult.OutputText))
                     return Result<List<string>>.Failure(Messages.BadRequest.WithTarget("NullValue"));


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
            

            var summarryResponse=new List<ModelGeneratedDesign>();
            foreach (var item in summariesList)
            {
                summarryResponse.Add(new ModelGeneratedDesign
                {
                    ModelChatId=payload.ChatId,
                    PromptUsed = item,
                       
                });
            }
            _context.AddRange(summarryResponse);
            await _context.SaveChangesAsync();

            return summariesList;
        }
        public async Task<Result<int>> CreateModelChate(string CustomerId)
        {
            var chat = new ModelChat
            {
                CustomerId = CustomerId,
                MaxChatTokens = 3
            };
            _context.Add(chat);
            await _context.SaveChangesAsync();

            return chat.ID;
        }
        public async Task<Result> AnalysisUserDocuments(string UserId, List<IFormFile> identityFiles)
        {

        }
    }
}
