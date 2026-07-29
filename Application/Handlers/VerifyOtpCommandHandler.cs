using Application.Commands;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers
{
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, Result<OtpResponse>>
    {
        private readonly IOtpService _otpService;
        private readonly UserManager<User> _userManager;
        private readonly D2DContext _context;

        public VerifyOtpCommandHandler(UserManager<User> userManager, IOtpService otpService, D2DContext context)
        {
            _userManager = userManager;
            _otpService = otpService;
            _context = context;
        }

        public async Task<Result<OtpResponse>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u=>u.Id == request.UserId);
            if (user == null)
                return Result<OtpResponse>.Failure(Messages.NotFound.WithTarget("User"));

            var existingOtp = await _context.Otps.AsNoTracking().FirstOrDefaultAsync(e => e.Code == request.Otp && e.UserId == request.UserId, cancellationToken);
            var isVerified = _otpService.VerifyOtp(existingOtp);
            if (!isVerified.IsSuccess)
                return Result<OtpResponse>.Failure(Messages.Expired.WithTarget("Otp"));

            existingOtp!.IsUsed = true;
            _context.Otps.Attach(existingOtp);
            _context.Entry(existingOtp).Property(o => o.IsUsed).IsModified = true;
            if (!user.EmailConfirmed)
            {

                user.EmailConfirmed = true;
                _context.Users.Attach(user);
                _context.Entry(user).Property(u => u.EmailConfirmed).IsModified = true;
            }
            if(request.flag)
                await _context.SaveChangesAsync(cancellationToken);

            return Result<OtpResponse>.Success(new OtpResponse { UserId=user.Id,UserType=user.UserType});
        }
    }
}