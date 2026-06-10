using Application.Commands;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.AuthDtos;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class VerifyMagicTokenCommandHandler : IRequestHandler<VerifyMagicTokenCommand, Result<JwtToken>>
    {
        private readonly D2DContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;

        public VerifyMagicTokenCommandHandler(D2DContext context, UserManager<User> userManager, IAuthService authService)
        {
            _context = context;
            _userManager = userManager;
            _authService = authService;
        }

        public async Task<Result<JwtToken>> Handle(VerifyMagicTokenCommand request, CancellationToken cancellationToken)
        {
            var token = _context.MagicTokens.FirstOrDefault(t => t.Token == request.Token);
            if (token == null || token.Expiration < DateTime.UtcNow || token.IsUsed)
                return Result<JwtToken>.Failure(Messages.Expired.WithTarget("MagicToken"));

            token.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);

            var user = await _userManager.FindByIdAsync(token.UserId);
            var newRefreshToken = await _authService.GenerateRefreshToken(token.UserId);
            var jwt = await _authService.GenerateAccessToken(user!);

            return Result<JwtToken>.Success(new JwtToken
            {
                UserID = token.UserId,
                AccessToken = jwt.Token,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            });
        }
    }
}