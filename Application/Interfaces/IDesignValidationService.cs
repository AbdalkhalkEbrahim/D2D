using Domain.DTOs;

namespace Application.Interfaces
{
    public interface IDesignValidationService
    {
        public Task<DesignValidationResponse> AnalyzeAsync(List<string> stepsUrls);
    }
}
