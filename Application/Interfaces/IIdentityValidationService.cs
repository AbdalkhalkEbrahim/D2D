using Application.Response;
using Domain.DTOs.ModelDtos;

namespace Application.Interfaces
{
    public interface IIdentityValidationService
    {
        Task<IdentityValidationResponse> AnalyzeAsync(string idFront, string idBack, string selfie);
        Task ValidateAndApproveUserIdentityAsync(string userId, string idFrontBase64, string idBackBase64, string selfieBase64);
    }
}
