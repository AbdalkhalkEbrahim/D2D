using Domain.Entities.Chats.AiModel;

namespace V02
{
    public interface IPromptBuilder
    {
        string Build(DesignState state);
    }
}
