using Application.Commands.ChatFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Google.Apis.Util;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ChatFeature
{
    public class SendOfferOtpCommandHandler : IRequestHandler<SendOfferOtpCommand, Result<OtpResponse>>
    {
        private readonly IOtpService _otpService;
        private readonly D2DContext _context;
        public SendOfferOtpCommandHandler(D2DContext context, IOtpService otpService)
        {
            _context = context;
            _otpService = otpService;
        }
        public async Task<Result<OtpResponse>> Handle(SendOfferOtpCommand request, CancellationToken cancellationToken)
        {
            var userEmail = await _context.Users
                .Where(u => (u.Id == request.CustomerId || u.Id == request.ProducerId)&&!u.IsDeleted)
                .Select(u => new {u.Id, u.Email }).ToListAsync();
            if(userEmail.Count != 2)
                return Result<OtpResponse>.Failure(Messages.NotFound.WithTarget("User"));

            string code = _otpService.GenerateOtp();
            var otp = new Otp
            {
                Code = code,
                ExpirationTime = DateTime.UtcNow.AddMonths(1),
                UserId = request.ProducerId
            };
            string email = userEmail.FirstOrDefault(u=>u.Id == request.CustomerId).Email;
            BackgroundJob.Enqueue<IEmailService>(emailService =>
              emailService.SendEmailAsync(email, "offer completion OTP ", $"Your code is {code}, and it's expired at {otp.ExpirationTime}"));

            await _context.AddAsync(otp);
            await _context.SaveChangesAsync();
            return  new OtpResponse { 
                
                UserId = request.ProducerId,
                UserType = UserType.Producer,
                Code = code,
                ExpirationTime = otp.ExpirationTime
            };
        }
    }
}
