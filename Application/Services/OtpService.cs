using Domain.Entities.Shared;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OtpService : IOtpService
    {
        private readonly D2DContext _context;

        public OtpService(D2DContext context)
        {
            _context = context;
        }

        //verify email forget pass producer otp
        public bool VerifyOtp(Otp? otp)
        {
            if (otp == null)
                throw new Exception("OTP not found");

            var result = true;

            if (otp == null|| otp.ExpirationTime < DateTime.UtcNow|| otp.IsUsed)
                result = false;


            return result;
        }
        public string GenerateOtp()
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specials = "!@#$%^&*()_+=-[]{}|<>?";

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


