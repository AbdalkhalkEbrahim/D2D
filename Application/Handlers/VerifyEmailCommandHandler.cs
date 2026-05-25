using Application.Commands;
using Domain.Entities.Shared;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, string>
    {
        private readonly IOtpService _otpService;
        private readonly UserManager<User> _userManager;
        private readonly D2DContext _context;


        public VerifyEmailCommandHandler(UserManager<User> userManager, IOtpService otpService, D2DContext context)
        {
            _userManager = userManager;
            _otpService = otpService;
            _context = context;
        }

        public async Task<string> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var existingOtp = await _context.Otps.FirstOrDefaultAsync(e => e.Code == request.Otp && e.UserId==request.UserId);
            var isVerified =  _otpService.VerifyOtp(existingOtp);
            if (!isVerified)
            {
                throw new Exception("OTP is invalid");
            }

            existingOtp!.IsUsed = true;
            user.EmailConfirmed = true;
            _context.Otps.Update(existingOtp);
            await _userManager.UpdateAsync(user);

            await _context.SaveChangesAsync(cancellationToken);
           
            return "Emai has been verified";
        }
    }
}
