using Application.Response;
using Domain.DTOs.ModelDtos;

namespace Application.Interfaces
{
    public interface IDesignValidationService
    {
        public Task<Result<DesignValidationResponse>> AnalyzeAsync(List<string> stepsUrls);
    }
}
