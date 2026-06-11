using Application.Response;
using Domain.Entities.Shared;

namespace Application.Interfaces
{
    public interface IOtpService
    {
        public Result<bool> VerifyOtp(Otp? otp);
        public string GenerateOtp();
    }
}
