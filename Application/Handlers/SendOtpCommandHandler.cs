using Application.Commands;
using Domain.Entities.Shared;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using static System.Net.WebRequestMethods;

public class SendOtpCommandHandler
    : IRequestHandler<SendOtpCommand, string>
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
        await _emailService.SendEmailAsync(request.Email, "OTP Verification", $"Your OTP is: {code}");
        return "OTP sent successfully";
    }

}