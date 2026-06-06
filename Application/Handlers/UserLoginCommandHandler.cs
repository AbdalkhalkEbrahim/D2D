using Application.Commands;
using Domain.DTOs;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace Application.Handlers
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, object>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public UserLoginCommandHandler(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<object> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            #region Rate Limit
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEndDate = await _userManager.GetLockoutEndDateAsync(user);
                var timeLeft = lockoutEndDate.Value.UtcDateTime - DateTime.UtcNow;
                throw new Exception($"Account is temporarily locked. Try again after {Math.Ceiling(timeLeft.TotalMinutes)} minutes.");
            }

            //var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

            //if (!result.Succeed)\
            if(!await _userManager.CheckPasswordAsync(user, request.Password))
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

                    throw new Exception($"Too many failed attempts. Account locked for {lockoutMinutes} minutes.");
                }

                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            await _userManager.SetLockoutEndDateAsync(user, null);
            #endregion

            if(user.IdentityStatus == VerificationStatus.Rejected)
            {
                throw new UnauthorizedAccessException("Account isn't Verified");
            }

            else if(user.IdentityStatus == VerificationStatus.Pending)
            {
               return new {
                    Status = "Pending"
                };
            }
            var AccessToken = await _authService.GenerateAccessToken(user);
            var refreshToken = await _authService.GenerateRefreshToken(user.Id);

            return new JwtToken
            {
                UserID = user.Id,
                AccessToken = AccessToken.Token,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = AccessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
            };
        }
    }
}