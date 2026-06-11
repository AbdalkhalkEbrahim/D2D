using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.AuthDtos;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, Result<JwtToken>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public UserLoginCommandHandler(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<Result<JwtToken>> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return Result<JwtToken>.Failure(Messages.NotFound.WithTarget("User"));
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);
                var timeLeft = lockoutEndDate.Value.UtcDateTime - DateTime.UtcNow;
                return Result<JwtToken>.Failure(Messages.AccountLocked(timeLeft.Minutes, timeLeft.Seconds));
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                await _userManager.AccessFailedAsync(user);
                var failedAttempts = await _userManager.GetAccessFailedCountAsync(user);

                if (failedAttempts >= 3)
                {
                    var previousLockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                    int lockoutMinutes = 5;

                    if (previousLockoutEnd.HasValue && previousLockoutEnd.Value > DateTimeOffset.UtcNow)
                    {
                        var previousDuration = previousLockoutEnd.Value - DateTimeOffset.UtcNow;
                        lockoutMinutes = (int)Math.Ceiling(previousDuration.TotalMinutes) * 2;
                    }

                    var lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(lockoutMinutes);
                    await _userManager.SetLockoutEndDateAsync(user, lockoutEnd);
                    return Result<JwtToken>.Failure(Messages.AccountLocked(lockoutMinutes));
                }

                return Result<JwtToken>.Failure(Messages.BadRequest.WithTarget("InvalidCredentials"));
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);

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