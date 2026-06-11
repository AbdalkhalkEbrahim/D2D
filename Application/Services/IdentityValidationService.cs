using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using Domain.DTOs;
using Domain.Interfaces;
using Application.Response;
using Application.Interfaces;
namespace Application.Services
{
    public class IdentityValidationService: IIdentityValidationService
    {
        private readonly ChatClient _chatClient;
        
        public IdentityValidationService(string openAI_APIKey)
        {
            var client = new OpenAIClient(openAI_APIKey);
            _chatClient = client.GetChatClient("gpt-5-mini");
        }

        public async Task<Result<IdentityValidationResponse>> AnalyzeAsync(string idFront, string idBack, string selfie)
        {
            var response =
                await _chatClient.CompleteChatAsync(
                [
                    new SystemChatMessage("""
            You are an expert AI Document Verification and Identity Fraud Detection Assistant. Your task is to audit three uploaded images: a Front ID, a Back ID, and a User Selfie. 

            Analyze the documents carefully and perform the following strict validation checks:

            1. Document Quality & Clarity: Inspect all images for blur, low resolution, missing edges, cut-off parts, glare, or unreadable text.
            2. Cross-Document ID Number Matching: 
               - Locate the national ID number on the Front ID (typically at the bottom right corner).
               - Locate the national ID number on the Back ID (typically at the top right corner).
               - Compare them. If they do not match exactly, you must immediately set "SimilarityScore": 0.0, and set "Notes": "ID numbers do not match".
            3. Visual Face Comparison: Estimate the structural similarity between the person in the Selfie and the photo on the Front ID based on visible facial features (e.g., eye distance, nose shape, jawline, face shape).
               - Provide a decimal score between 0.0 and 1.0 for "SimilarityScore".
               - If a visual comparison is completely impossible due to severe blur or hidden faces, return null for "SimilarityScore".
            4. Manual Review Trigger: Set "NeedsManualReview" to true if there is an ID mismatch, low document quality, or a low similarity score (< 0.7).

            You must return ONLY a valid JSON object matching the schema below. Do not include any conversational introduction, markdown code block wrappers (like ```json), or trailing text.

            Expected Output JSON Format:
            {
              "SimilarityScore": 0.0, 
              "DocumentQuality": "Excellent / Good / Poor (Specify reasons if poor, e.g., Blur, Unreadable Text)",
              "Notes": "Clear detailed findings about the matching results, quality issues, or anomalies found.",
              "NeedsManualReview": false
            }
            """),

            new UserChatMessage(ChatMessageContentPart.CreateImagePart(new Uri(idFront))),

            new UserChatMessage(ChatMessageContentPart.CreateImagePart(new Uri(idBack))),

            new UserChatMessage(ChatMessageContentPart.CreateImagePart(new Uri(selfie)))
                ]);
            string rawResponse = response.Value.Content[0].Text;
            return Result<IdentityValidationResponse>.Success(AIResponseMapper.Map<IdentityValidationResponse>(rawResponse));
        }
    }
}
