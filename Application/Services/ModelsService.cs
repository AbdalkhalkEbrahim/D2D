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
using Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Domain.Enums.Status;
using Domain.DTOs;

namespace Application.Services
{
    public class ModelsService:IModelesService
    {
        private readonly D2DContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUploadService _uploadService;
        private readonly string SystemGenerationPrompt = """
            You are an AI identity verification assistant.

            You will receive exactly three images in the following order:

            Image 1: National ID Card - Front Side
            Image 2: National ID Card - Back Side
            Image 3: User Selfie

            The verification process consists of TWO COMPLETELY INDEPENDENT stages.

            ==================================================
            STAGE 1 - NATIONAL ID VERIFICATION
            ==================================================

            IMPORTANT:

            For this stage, IGNORE the User Selfie completely.

            Use ONLY:
            - National ID Front
            - National ID Back

            Your tasks:

            1. Extract the Egyptian National ID number from the front image.
            2. Extract the Egyptian National ID number from the back image.

            The Egyptian National ID always contains exactly 14 digits.

            The digits may appear using Eastern Arabic numerals (٠١٢٣٤٥٦٧٨٩).

            Before comparing or returning the IDs:
            - Convert Eastern Arabic numerals to Western numerals (0123456789).
            - Remove spaces.
            - Remove dots.
            - Remove commas.
            - Remove dashes.
            - Remove slashes.
            - Remove every non-digit character.
            - Return ONLY English digits.
            - The final ID must contain exactly 14 digits.

            If either ID cannot be confidently extracted as a valid 14-digit number, return ONLY:

            {
                "success": false,
                "nationalIdMatched": false,
                "reason": "Unable to confidently extract a valid Egyptian National ID.",
                "frontNationalId": null,
                "backNationalId": null
            }

            Do NOT continue.

            If the two National IDs are different, return ONLY:

            {
                "success": false,
                "nationalIdMatched": false,
                "frontNationalId": "<front_id>",
                "backNationalId": "<back_id>",
                "message": "The National ID numbers do not match."
            }

            Do NOT continue.

            ==================================================
            STAGE 2 - FACE VERIFICATION
            ==================================================

            Execute this stage ONLY if Stage 1 completed successfully.

            DO NOT re-check the National ID.
            DO NOT modify the extracted National ID.
            DO NOT fail because the User Selfie belongs to another person.

            The User Selfie is ONLY used for face comparison.

            Use ONLY:
            - National ID Front
            - User Selfie

            Compare the person's face on the National ID Front with the uploaded User Selfie.

            If the faces belong to different people, DO NOT return an OCR failure.

            Instead, return success=true, nationalIdMatched=true, and set samePerson=false with an appropriate similarity score.

            Return ONLY:

            {
                "success": true,
                "nationalIdMatched": true,
                "nationalId": "<14_digit_national_id>",
                "verificationReport": {
                "samePerson": true,
                "faceSimilarity": 96,
                "confidence": "Very High",
                "decision": "Likely Match",
                "faceVisibility": "Excellent",
                "idImageQuality": "Good",
                "selfieImageQuality": "Good",
                "blurDetected": false,
                "glareDetected": false,
                "occlusionDetected": false,
                "faceOrientation": "Frontal",
                "lightingQuality": "Good",
                "possibleIssues": [],
                "observations": [
                    "The facial features are highly consistent."
                ],
                "summary": "The uploaded selfie appears highly similar to the person shown on the National ID card."
                }
            }

            Rules:
            - The User Selfie MUST NEVER be used to extract or validate the National ID.
            - National ID extraction depends ONLY on the Front and Back images.
            - Face comparison depends ONLY on the Front image and the User Selfie.
            - If the selfie belongs to another person, still return the extracted National ID and the verification report with samePerson=false.
            - Never return an OCR extraction failure because of the selfie.
            - Never return Markdown.
            - Return valid JSON only.
            - Never explain your reasoning.
            - Never invent National ID numbers.
            - Never guess missing digits.
            - Never return Arabic numerals.
            - The National ID must contain exactly 14 English digits.
            """;
        private const string LicensePrompt = """
            You are an AI Egyptian factory document verification assistant.

            You will receive one or more images submitted by a factory.

            Each image represents a separate official document.
            Analyze EACH image independently first, then perform an overall comparison between all documents.

            The documents may include:

            - السجل الصناعي (Industrial Registration)
            - السجل التجاري (Commercial Registration)
            - البطاقة الضريبية (Tax Card)
            - رخصة تشغيل المصنع (Factory Operating License)
            - رخصة صناعية
            - Any official factory-related document

            ==================================================
            STEP 1 - INDIVIDUAL IMAGE ANALYSIS
            ==================================================

            For EACH uploaded image separately:

            Perform the following:

            1. Identify the document type.

            Possible values:

            - Industrial Registration
            - Commercial Registration
            - Tax Card
            - Factory Operating License
            - Industrial License
            - Unknown

            2. Extract all visible information from THIS IMAGE ONLY.

            Extract:

            - Company Name
            - Factory Name
            - Owner Name
            - National ID / Owner ID if visible
            - Commercial Registration Number
            - Industrial Registration Number
            - Tax Number
            - License Number
            - Issue Date
            - Expiry Date
            - Issuing Authority
            - Factory Activity
            - Factory Address

            3. Analyze the visual quality of THIS IMAGE.

            Check:

            - Blur
            - Cropping
            - Missing edges
            - Missing pages
            - Image clarity
            - Visible edits
            - Different fonts
            - Suspicious formatting
            - Missing stamps
            - Missing signatures

            If any information is unreadable:

            Return null.

            Never guess values.

            ==================================================
            STEP 2 - DOCUMENTS COMPARISON
            ==================================================

            After analyzing all images individually:

            Compare the extracted information between documents.

            Check:

            - Company name consistency
            - Factory name consistency
            - Address consistency
            - Registration numbers consistency
            - Tax information consistency
            - Factory activity consistency
            - Whether all documents belong to the same factory

            ==================================================
            STEP 3 - FACTORY ACTIVITY VERIFICATION
            ==================================================

            Determine whether the factory activity is related to clothing manufacturing.

            Relevant activities include:

            - تصنيع الملابس الجاهزة
            - صناعة الملابس
            - صناعة المنسوجات
            - الغزل والنسيج
            - Apparel Manufacturing
            - Garment Manufacturing
            - Textile Manufacturing

            ==================================================
            OUTPUT FORMAT
            ==================================================

            Return ONLY valid JSON:

            {
              "success": true,

              "imagesAnalysis": [
                {
                  "imageNumber": 1,
                  "documentType": "Industrial Registration",

                  "extractedData": {
                    "companyName": "",
                    "factoryName": "",
                    "ownerName": "",
                    "nationalId": "",
                    "commercialRegistrationNumber": "",
                    "industrialRegistrationNumber": "",
                    "taxNumber": "",
                    "licenseNumber": "",
                    "issueDate": "",
                    "expiryDate": "",
                    "issuingAuthority": "",
                    "factoryActivity": "",
                    "factoryAddress": ""
                  },

                  "imageQuality": {
                    "quality": "Good",
                    "blurDetected": false,
                    "cropped": false,
                    "editedAppearance": false,
                    "missingParts": false
                  },

                  "possibleIssues": []
                }
              ],

              "overallAnalysis": {
                "isRelevant": true,

                "documentsBelongToSameFactory": true,

                "crossCheck": {
                  "companyNameMatched": true,
                  "addressMatched": true,
                  "activityMatched": true,
                  "numbersConsistent": true
                },

                "confidence": "High",

                "summary": "The submitted documents appear consistent and related to a clothing manufacturing factory."
              }
            }

            ==================================================
            RULES
            ==================================================

            - Analyze every image separately before comparison.
            - Do not mix information between images.
            - Do not invent missing values.
            - Return null for unreadable fields.
            - Return valid JSON only.
            - Do not use Markdown.
            - Do not explain your reasoning.
            - Do not claim legal authenticity.
            - Only evaluate visible information.
            """;
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
        public async Task<Result> AnalysisUserDocuments(string userId, List<QwenImageItem> identityFiles,List<FileUploadModel> uploadedDocuments)
        {
           if (identityFiles == null || identityFiles.Count != 3)
                return Result.Failure(Messages.BadRequest.WithTarget("Default"));

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Result.Failure(Messages.NotFound.WithTarget("User"));

            var images = identityFiles;

           

            var requestBody = new QwenMultimodalRequest
            {
                ModelId = _configuration["AiKey:Model_description_image"] ?? "qwen.qwen3-vl-235b-a22b",
                Messages = new List<QwenMessage>
                {
                    new QwenMessage
                    {
                        Role = "user",
                        Text = SystemGenerationPrompt,
                        Images = images
                    }
                }
            };

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    _configuration["AiKey:ApiKey"]);

            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            string endpoint = _configuration["AiKey:EndPoint_description_image"]
                ?? "http://apiaccess.iti.net.eg/api/v1/student/multimodal-chat";

            var response = await client.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                string failedBody = await response.Content.ReadAsStringAsync();
                return Result.Failure(Messages.ModelSummaryError(failedBody));
            }

            var json = await response.Content.ReadAsStringAsync();

            var aiResponse = JsonSerializer.Deserialize<QwenMultimodalResponse>(json);

            if (aiResponse == null)
                return Result.Failure(Messages.BadRequest.WithTarget("NullValue"));

            var report = JsonSerializer.Deserialize<IdentityVerificationResponse>(aiResponse.OutputText);

            if (report == null)
                return Result.Failure(Messages.BadRequest.WithTarget("NullValue"));

            if (_context.Users.Any(u => u.NationalId == report.NationalId))
                return Result.Failure(Messages.Conflict.WithTarget("Default"));


            if (!report.NationalIdMatched )
            {
                user.IdentityStatus = VerificationStatus.Rejected;

                await _context.SaveChangesAsync();

                return Result.Success();
            }



            var uploads =( await _uploadService.UploadFileAsync(uploadedDocuments)).Value;

            var identity = new UserIdentityFiles
            {
                UserId = userId,
                NationalId = report.NationalId,
                IsSuccess = report.Success,
                IsSamePerson = report.VerificationReport!.SamePerson,
                FaceSimilarity = report.VerificationReport.FaceSimilarity,
                Confidence = report.VerificationReport.Confidence,
                Observations = report.VerificationReport.Observations,
                FrontImageID = uploads[0],
                BackImageID = uploads[1],
                PersonalImage = uploads[2]
            };

            _context.UserIdentityFiles.Add(identity);

            if (!report.VerificationReport.SamePerson &&
                report.VerificationReport.FaceSimilarity < 30)
            {
                user.IdentityStatus = VerificationStatus.Rejected;
            }
           

            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
