using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IIdentityValidationService
    {
        Task<IdentityValidationResponse> AnalyzeAsync(string idFront, string idBack, string selfie);
    }
}
