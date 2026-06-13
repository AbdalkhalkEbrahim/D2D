using Application.Commands;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;

public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, Result<OtpResponse>>
{
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly D2DContext _context;

    public SendOtpCommandHandler(IOtpService otpService, IEmailService emailService, D2DContext context)
    {
        _otpService = otpService;
        _emailService = emailService;
        _context = context;
    }

    public async Task<Result<OtpResponse>> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (user == null)
            return Result<OtpResponse>.Failure(Messages.NotFound.WithTarget("User"));

        if (user.OtpLockoutEnd.HasValue && user.OtpLockoutEnd.Value > DateTimeOffset.UtcNow)
        {
            var duration = user.OtpLockoutEnd;
            var timeLeft = duration.Value.UtcDateTime - DateTime.UtcNow;
            return Result<OtpResponse>.Failure(Messages.OtpBackoff(timeLeft.Minutes, timeLeft.Seconds));
        }

        user.OtpLockoutCount = user.OtpLockoutCount ?? 1;
        user.OtpLockoutEnd = DateTimeOffset.UtcNow.AddMinutes((double)user.OtpLockoutCount);

        _context.Users.Update(user);

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

        return Result<OtpResponse>.Success(new OtpResponse { UserId=user.Id,UserType=user.UserType});
    }
}