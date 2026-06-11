using Application.Response;
using Domain.DTOs.ModelDtos;

namespace Application.Interfaces
{
    public interface IIdentityValidationService
    {
        Task<Result<IdentityValidationResponse>> AnalyzeAsync(string idFront, string idBack, string selfie);
    }
}
