using Application.Interfaces;
using Domain.DTOs;
using Domain.DTOs.Model;
using Domain.Entities.Chats.AiModel;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Presentation.Controllers;
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
    public class ChatModelController : BaseApiController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;
        private readonly IModelesService _modelesService;

        private const string SystemGenerationPrompt =
            "Describe this image in detailes, seperate the description into 2 section, " +
            "one for the design front and the other fot the design back as when passing " +
            "this description to another model it can regenrate a very similar design to it. " +
            "descripe the shapes or axs of anything in the image.just a plain text without " +
            "any characters focus on the design itself with colors' degree, logos, lines " +
            "and how it looks and the positions of everything the size and thickness of " +
            "any printed logo or lines or drawings, descriping how it looks detailly";

        public ChatModelController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IUploadService uploadService,IModelesService modelesService)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _uploadService = uploadService;
            _modelesService = modelesService;
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

                if (itiResult == null /*|| string.IsNullOrEmpty(itiResult.OutputText)*/)
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
        [HttpPost("create-model-chat")]
        
        public async Task<IActionResult> CreateModelChat([FromBody] string CustomerId)
        {
            var response = await _modelesService.CreateModelChate(CustomerId);
            return HandleResult(response);
        }
        [HttpPost("generate-summaries")]
        public async Task<IActionResult> GenerateSummaries([FromBody] SummaryGenerationPayload payload)
        {
            var response=await _modelesService.GenerateSummaries(payload);
            return Ok(response);
        }

        [HttpPost("generate-design-image")]
        public async Task<IActionResult> GenerateDesignImage([FromBody] string summaryDescription, string userId,int chatId)
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
                     ModelChatId=chatId,
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


       

       

       

        
       

      

       

        public class FluxGenerationRequest
        {
            [JsonPropertyName("inputs")]
            public string Inputs { get; set; }
        }

       
    }
}