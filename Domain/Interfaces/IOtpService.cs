using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IOtpService
    {
        public bool VerifyOtp(Otp? otp);
       public string GenerateOtp();
    }
}
