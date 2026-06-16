using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.AuthDtos;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, Result<object>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly D2DContext _context;

        public UserLoginCommandHandler(UserManager<User> userManager, IAuthService authService, D2DContext context)
        {
            _userManager = userManager;
            _authService = authService;
            _context = context;
        }

        public async Task<Result<object>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email == request.Email);

            if (user == null)
            {
                return Result<JwtToken>.Failure(Messages.NotFound.WithTarget("User"));
            }

            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow)
            {
                var timeLeft = user.LockoutEnd.Value - DateTimeOffset.UtcNow;

                int minutesLeft = (int)Math.Ceiling(timeLeft.TotalMinutes);
                int secondsLeft = timeLeft.Seconds; 

                return Result<JwtToken>.Failure(Messages.AccountLocked(minutesLeft, secondsLeft));
            }

            var passwordHasher = new PasswordHasher<User>();
            var verificationResult = passwordHasher.VerifyHashedPassword(user,user.PasswordHash,request.Password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {

                var failedAttempts = ++user.AccessFailedCount;

                if (failedAttempts >= 3)
                {
                    var previousLockoutEnd = user.LockoutEnd;

                    var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(5);
                    user.LockoutEnabled = true;
                    await _context.SaveChangesAsync();
                    return Result<JwtToken>.Failure(Messages.AccountLocked(5));
                }

                return Result<JwtToken>.Failure(Messages.BadRequest.WithTarget("InvalidCredentials"));
            }

            user.AccessFailedCount = 0;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync();

            if (!user.EmailConfirmed)
                return Result<object>.Success(new {message ="Redirect to sned otp", Id = user.Id});

            if (user.FrontImageID is null)
                return Result<object>.Success(new { message = "Redirect to identity uploading", Id = user.Id, role = user.UserType });


            if (user.IdentityStatus == VerificationStatus.Rejected)
            {
                return Result<JwtToken>.Failure(Messages.AccountStatus.WithTarget("Rejected"));
            }
            else if (user.IdentityStatus == VerificationStatus.Pending)
            {
                return Result<JwtToken>.Failure(Messages.AccountStatus.WithTarget("Pending"));
            }

            var accessToken = await _authService.GenerateAccessToken(user);
            var refreshToken = await _authService.GenerateRefreshToken(user.Id);

            return Result<JwtToken>.Success(new JwtToken
            {
                UserID = user.Id,
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Value.Token,
                AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshToken.Value.ExpiresAt,
            });
        }
    }
}