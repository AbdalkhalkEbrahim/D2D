using Application.Interfaces;
using Application.Response;
using Domain.DTOs.ModelDtos;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace Application.Services
{
    public class DesignValidationService:IDesignValidationService
    {
        private readonly ChatClient _chatClient;

        public DesignValidationService(IConfiguration configuration)
        {
            var client = new OpenAIClient(configuration["OpenAI:ApiKey"]);

            _chatClient = client.GetChatClient("gpt-5-mini");
        }

        public async Task<Result<DesignValidationResponse>> AnalyzeAsync(List<string> stepsUrls)
        {
            var userMessage = new UserChatMessage();
            userMessage.Content.Add(ChatMessageContentPart.CreateTextPart("Here are the images representing the stages of the clothing design process:"));

            foreach (var url in stepsUrls)
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    userMessage.Content.Add(ChatMessageContentPart.CreateImagePart(new Uri(url)));
                }
            }

            var response = await _chatClient.CompleteChatAsync(
            [
                new SystemChatMessage("""
            You are an expert AI Forensic Auditor specializing in the Fashion and Apparel Industry, focusing on design authenticity, intellectual property verification, and plagiarism detection.
            Your task is to analyze a series of step-by-step images provided by a designer to verify whether they represent an authentic, continuous, and logical workflow of creating a single, cohesive clothing design. You must also detect any indicators of fraud, theft, or inconsistent ownership.

            Carefully evaluate the images using the following criteria:
            1. Visual Continuity: Check if the fabric textures, colors, patterns, and stitching styles remain strictly consistent across all steps.
            2. Workflow Logic: Ensure the progression represents a realistic design lifecycle (e.g., Sketch -> Pattern/Drafting -> Cutting -> Sewing/Prototyping -> Final Product). Detect if any intermediate stages are abruptly skipped.
            3. Environment & Background: Examine the physical workspace, background lighting, mannequin/model types, and tools visible in the frame. Drastic, unexplained shifts suggest the images were aggregated from different sources.
            4. Watermarks & Digital Metadata: Scan all images thoroughly for overlapping watermarks, logos, text overlays, or blurry patches that indicate an attempt to hide another creator's identity or platform branding.
            5. Inconsistencies: Highlight any structural discrepancies (e.g., a collar style or zipper placement changing mysteriously between steps).

            You must return ONLY a valid JSON object matching the schema below. Do not include any conversational introduction, markdown code block wrappers (like ```json), or trailing text.

            Expected Output JSON Format:
            {
              "OwnershipVerified": false,
              "ConfidenceScore": 0 (from 0 to 1),
              "DetectedRisks": "String describing any red flags, watermarks, or mismatched backgrounds found, or 'None'",
              "ProgressScore": 0 (from 0 to 100),
              "DetailedExplanation": "string Analysis of the continuity and workflow, Findings regarding branding, text overlays, or theft indicators, Clear summary of why ownership is verified or rejected"
            }
            """),

        userMessage
            ]);
            string rawResponse = response.Value.Content[0].Text;
            return Result<DesignValidationResponse>.Success(AIResponseMapper.Map<DesignValidationResponse>(rawResponse));
        }
    }
}
