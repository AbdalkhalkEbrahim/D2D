using Application.Commands;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs;
using Domain.Entities.Shared;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, Result<OtpResponse>>
{
    private readonly IOtpService _otpService;
    private readonly D2DContext _context;

    public SendOtpCommandHandler(IOtpService otpService, D2DContext context)
    {
        _otpService = otpService;
        _context = context;
    }

    public async Task<Result<OtpResponse>> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var totalWatch = Stopwatch.StartNew();
        var stepWatch = Stopwatch.StartNew();

        var user = await _context.Users
            .AsNoTracking()
            .Select(u => new { u.Id, u.Email, u.UserType, u.OtpLockoutEnd, u.OtpLockoutCount })
            .FirstOrDefaultAsync(u => u.Id == request.ID, cancellationToken);

        stepWatch.Stop();
        Console.WriteLine($"[PERF] 1. Fetch User took: {stepWatch.ElapsedMilliseconds}ms");

        if (user == null)
            return Result<OtpResponse>.Failure(Messages.NotFound.WithTarget("User"));

        if (!request.flag)
        {
            var email = await _context.Users.Select(u => u.Email).FirstOrDefaultAsync(e => e == request.Email);
            if(email != null)
               return Result<OtpResponse>.Failure(Messages.Conflict.WithTarget("Email"));

        }

        if (user.OtpLockoutEnd.HasValue && user.OtpLockoutEnd.Value > DateTimeOffset.UtcNow)
        {
            var timeLeft = user.OtpLockoutEnd.Value.UtcDateTime - DateTime.UtcNow;
            return Result<OtpResponse>.Failure(Messages.OtpBackoff(timeLeft.Minutes, timeLeft.Seconds));
        }

        var currentCount = user.OtpLockoutCount ?? 1;
        var newLockoutEnd = DateTimeOffset.UtcNow.AddMinutes((double)currentCount);

        stepWatch.Restart();
        await _context.Users
            .Where(u => u.Id == user.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.OtpLockoutEnd, newLockoutEnd)
                .SetProperty(u => u.OtpLockoutCount, currentCount),
                cancellationToken);

/*        _context.Attach(user);
        _context.Entry(user).Property(u=>u.OtpLockoutCount).IsModified = true;*/

        stepWatch.Stop();
        Console.WriteLine($"[PERF] 2. ExecuteUpdateAsync (Lockout) took: {stepWatch.ElapsedMilliseconds}ms");

        stepWatch.Restart();
        var code = _otpService.GenerateOtp();
        var otp = new Otp
        {
            Code = code,
            UserId = user.Id,
            ExpirationTime = DateTime.UtcNow.AddMinutes(5),
            IsUsed = false
        };

        stepWatch.Stop();
        Console.WriteLine($"[PERF] 3. OTP Generation in memory took: {stepWatch.ElapsedMilliseconds}ms");

        stepWatch.Restart();
        await _context.Otps.AddAsync(otp, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        stepWatch.Stop();
        Console.WriteLine($"[PERF] 4. SaveChangesAsync (Insert OTP) took: {stepWatch.ElapsedMilliseconds}ms");

        stepWatch.Restart();
        BackgroundJob.Enqueue<IEmailService>(emailService =>
            emailService.SendEmailAsync(request.Email, "Your OTP Code", $"Your code is {code}"));
        stepWatch.Stop();
        Console.WriteLine($"[PERF] 5. Hangfire Enqueue took: {stepWatch.ElapsedMilliseconds}ms");

        totalWatch.Stop();
        Console.WriteLine($"[PERF] === TOTAL HANDLER TIME: {totalWatch.ElapsedMilliseconds}ms ===");

        return Result<OtpResponse>.Success(new OtpResponse { UserId = user.Id, UserType = user.UserType,Code = code});
    }
}