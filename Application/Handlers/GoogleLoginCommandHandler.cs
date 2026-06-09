using Application.Commands;
using Domain.DTOs;
using Domain.Entities.Shared;
using Domain.Interfaces;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
namespace Application.Handlers
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, JwtToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public GoogleLoginCommandHandler(UserManager<User> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<JwtToken> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(request.IdToken, new GoogleJsonWebSignature.ValidationSettings());
            }
            catch (Exception)
            {
                throw new Exception("Invalid Google token.");
            }

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
                throw new Exception("user not found, register first"); 

           /* if (user == null)
            {
                user = new User
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    EmailConfirmed = true,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    UserType = request.UserType,
                    BD = new DateTime(request.Year, request.Month, request.Day),
                 //   AnonName = user.AnonymousName(request.UserType)
                };
                user.AnonName = user.AnonymousName(request.UserType);
                if (!user.IsAllowed)
                    throw new Exception("not allowed age to register.");

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    throw new Exception("Failed to create user from Google account.");
                }

                var identityUserLogin = new UserLoginInfo("Google", payload.Subject, "Google");
                await _userManager.AddLoginAsync(user, identityUserLogin);
            }
            else*/
            {
                
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
            }

            var accessToken = await _authService.GenerateAccessToken(user);
            var refreshToken = await _authService.GenerateRefreshToken(user.Id);

            return new JwtToken
            {
                UserID = user.Id,
                AccessToken = accessToken.Token,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = accessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt
            };
        }
    }
}