using Application.Commands;
using Domain.Entities.Shared;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using static System.Net.WebRequestMethods;

public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, string>
{
    private readonly UserManager<User> _userManager;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly D2DContext _context;

    public SendOtpCommandHandler(UserManager<User> userManager, IOtpService otpService, IEmailService emailService, D2DContext context)
    {
        _userManager = userManager;
        _otpService = otpService;
        _emailService = emailService;
        _context = context;
    }

    public async Task<string> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new Exception("User not found");

        #region Exponential Backoff
        if (user.OtpLockoutEnd.HasValue && user.OtpLockoutEnd.Value > DateTimeOffset.UtcNow)
        {
            var timeLeft = user.OtpLockoutEnd.Value - DateTimeOffset.UtcNow;
            throw new Exception($"Please wait {Math.Ceiling(timeLeft.TotalMinutes)} minutes before requesting a new OTP.");
        }

        int nextLockoutMinutes = 1;

        if (user.OtpLockoutEnd.HasValue)
        {
            var timeSinceLockoutEnded = DateTimeOffset.UtcNow - user.OtpLockoutEnd.Value;

            if (timeSinceLockoutEnded.TotalMinutes > 60)
            {
                nextLockoutMinutes = 1;
                user.OtpLockoutEnd = null;
            }
            else
            {
                var previousDuration = user.OtpLockoutEnd.Value - DateTimeOffset.UtcNow;
                int previousMinutes = (int)Math.Abs(Math.Ceiling(previousDuration.TotalMinutes));
                nextLockoutMinutes = previousMinutes * 2;
            }
        }

        user.OtpLockoutEnd = DateTimeOffset.UtcNow.AddMinutes(nextLockoutMinutes);
        _context.Users.Update(user);
        #endregion

        var code = _otpService.GenerateOtp();

        var otp = new Otp
        {
            Code = code,
            UserId = user.Id,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false
        };

        await _context.Otps.AddAsync(otp, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        //alter email formula
        await _emailService.SendEmailAsync(request.Email, "OTP Verification", $"Your OTP is: {code}");

        return "OTP sent successfully"; //userID, UserType
    }

}