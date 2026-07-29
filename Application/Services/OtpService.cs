using Application.Interfaces;
using Application.Response;
using Domain.Entities.Shared;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class OtpService : IOtpService
    {

        //verify email forget pass producer otp
        public Result<bool> VerifyOtp(Otp? otp)
        {

            if (otp == null || /*otp.ExpirationTime < DateTime.UtcNow */ otp.IsUsed)
               return Result<bool>.Failure(Messages.Expired.WithTarget("otp"));

            return Result<bool>.Success(true);
        }
        public string GenerateOtp()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specials = "!@#$%^&*?";

            var otpBuilder = new StringBuilder();

            var poolSelection = new List<string> { upper, lower, digits, specials };

            for (int i = 0; i < 6; i++)
            {
                int chosenPoolIndex = RandomNumberGenerator.GetInt32(0, poolSelection.Count);
                string currentPool = poolSelection[chosenPoolIndex];

                int characterIndex = RandomNumberGenerator.GetInt32(0, currentPool.Length);
                char secureChar = currentPool[characterIndex];

                otpBuilder.Append(secureChar);
            }
            //Console.WriteLine("New OTP" + otpBuilder);
            return otpBuilder.ToString();
        }
    }
}