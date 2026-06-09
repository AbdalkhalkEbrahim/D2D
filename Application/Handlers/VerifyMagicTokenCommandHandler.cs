using Application.Commands;
using Domain.DTOs;
using Domain.Entities.Shared;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class VerifyMagicTokenCommandHandler : IRequestHandler<VerifyMagicTokenCommand, JwtToken>
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
        public async Task<JwtToken> Handle(VerifyMagicTokenCommand request, CancellationToken cancellationToken)
        {
            var token = _context.MagicTokens.FirstOrDefault(t => t.Token == request.Token);
            if (token == null || token.Expiration < DateTime.UtcNow || token.IsUsed)
            {
                throw new Exception("Invalid or expired magic token.");
            }
            token.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);
            var user = await _userManager.FindByIdAsync(token.UserId);
            var newRefreshToken = await _authService.GenerateRefreshToken(token.UserId);
            var jwt = await _authService.GenerateAccessToken(user!);
            return new JwtToken
            {
                UserID = token.UserId,
                AccessToken = jwt.Token,
                RefreshToken = newRefreshToken.Token,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(15),
                RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
            };
        }
    }
}
