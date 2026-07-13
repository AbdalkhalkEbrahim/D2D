using Domain.Entities.Chats.AiModel;

namespace V02.Services
{
    public interface IImageGenerationService
    {
        public Task<string> GenerateOrEditImageAsync(string prompt, DesignState state);
    }
}
