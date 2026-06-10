using Domain.Entities.Shared;

namespace Application.Interfaces
{
    public interface IOtpService
    {
        public bool VerifyOtp(Otp? otp);
        public string GenerateOtp();
    }
}
