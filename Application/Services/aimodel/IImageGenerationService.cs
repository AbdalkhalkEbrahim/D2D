namespace V02.Services
{
    public interface IImageGenerationService
    {
        Task<string> GenerateImageAsync(string enhancedPrompt);
    }
}
