
using Domain.Entities.Chats.AiModel;

namespace V02
{
    public class PromptBuilder : IPromptBuilder
    {
        public string Build(DesignState state)
        {
            var parts = new List<string>();

            parts.Add($"{state.Fit} {state.Type}");

            if (!string.IsNullOrWhiteSpace(state.PrimaryColor))
                parts.Add($"{state.PrimaryColor} color");

            if (!string.IsNullOrWhiteSpace(state.Material))
                parts.Add($"{state.Material} fabric");

            if (!string.IsNullOrWhiteSpace(state.Theme))
                parts.Add($"{state.Theme} style");

            parts.AddRange(state.Details);

            parts.Add("Front and back view");
            parts.Add("White studio background");

            return string.Join(", ", parts);
        }
    }
}
