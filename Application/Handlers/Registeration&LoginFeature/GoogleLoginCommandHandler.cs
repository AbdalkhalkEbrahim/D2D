using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.AuthDtos;
using Domain.Entities.Shared;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, Result<JwtToken>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public GoogleLoginCommandHandler(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<Result<JwtToken>> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings());
            }
            catch (Exception)
            {
                return Result<JwtToken>.Failure(Messages.BadRequest.WithTarget("InvalidCredentials"));
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
                return Result<JwtToken>.Failure(Messages.NotFound.WithTarget("User"));

            var logins = await _userManager.GetLoginsAsync(user);
            bool isLinkedToGoogle = false;
            foreach (var login in logins)
            {
                if (login.LoginProvider == "Google")
                {
                    isLinkedToGoogle = true;
                    break;
                }
            }

            if (!isLinkedToGoogle)
            {
                var identityUserLogin = new UserLoginInfo("Google", payload.Subject, "Google");
                await _userManager.AddLoginAsync(user, identityUserLogin);
            }

            var accessToken = await _authService.GenerateAccessToken(user);
            var refreshToken = await _authService.GenerateRefreshToken(user.Id);

            return Result<JwtToken>.Success(new JwtToken
            {
                UserID = user.Id,
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Value.Token,
                AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshToken.Value.ExpiresAt
            });
        }
    }
}