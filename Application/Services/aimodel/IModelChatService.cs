
using Domain.Entities.Chats.AiModel;

namespace V02.Services
{
    public interface IModelChatService
    {
        Task<DesignState> EnhancePromptAsync(string userMessage);
        public Task<DesignState> UpdateDesignStateAsync(DesignState currentState, string userRequest);

    }
}
