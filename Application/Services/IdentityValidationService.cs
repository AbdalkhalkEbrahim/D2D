using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using Domain.DTOs;
using Domain.Interfaces;
namespace Application.Services
{
    public class IdentityValidationService: IIdentityValidationService
    {
        private readonly ChatClient _chatClient;

        public IdentityValidationService(IConfiguration configuration)
        {
            var client = new OpenAIClient(configuration["OpenAI:ApiKey"]);
            _chatClient = client.GetChatClient("gpt-5-mini");
        }

        public async Task<IdentityValidationResponse> AnalyzeAsync(string idFront, string idBack, string selfie)
        {
            var response =
                await _chatClient.CompleteChatAsync(
                [
                    new SystemChatMessage("""
            Analyze the uploaded identity documents.

            Tasks:
            - Check if the ID images are clear
            - Check whether the selfie appears to match the ID owner
            - Detect obvious issues (blur, missing parts, unreadable text)
            - Even if you cannot perform biometric face recognition, estimate the structural similarity between the person in the selfie and the ID photo based on visible features (eyes, nose, face shape). If completely impossible due to image quality, return null. But do your best to provide a decimal score between 0.0 and 1.0
            Return valid JSON:

            {
                "SimilarityScore":0,
                "DocumentQuality":"",
                "Notes":"",
                "NeedsManualReview":false
            }
            """),

            new UserChatMessage(ChatMessageContentPart.CreateImagePart(new Uri(idFront))),

            new UserChatMessage(ChatMessageContentPart.CreateImagePart(new Uri(idBack))),

            new UserChatMessage(ChatMessageContentPart.CreateImagePart(new Uri(selfie)))
                ]);
            // Extract the raw text response from the AI (unstructured format)
            string rawResponse = response.Value.Content[0].Text;
            // Map the raw response to the IdentityValidationResponse
            return AIResponseMapper.Map<IdentityValidationResponse>(rawResponse);
        }
    }
}
