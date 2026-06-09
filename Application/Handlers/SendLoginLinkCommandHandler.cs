using Application.Commands;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class SendLoginLinkCommandHandler : IRequestHandler<SendLoginLinkCommand, string>
    {
        private readonly D2DContext _context;
        private readonly IEmailService _emailService;
        public SendLoginLinkCommandHandler(D2DContext context, IEmailService emailService) 
        {
            _context = context;
            _emailService = emailService;
        }
        public async Task<string> Handle(SendLoginLinkCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserID);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            user.MagicToken = new MagicToken
            {
                Token = Convert.ToHexString(bytes).ToLower(),
                Expiration = DateTime.UtcNow.AddHours(1),
                IsUsed = false,
                UserId = user.Id

            };
            user.IdentityStatus = VerificationStatus.Approved;
            _context.MagicTokens.Add(user.MagicToken);
            await _context.SaveChangesAsync();
            await _emailService.SendEmailAsync(user.Email!, "Your Login Link", $"Click the link to login: https://d2dplatform.runasp.net/api/verify-magic-token?token={user.MagicToken.Token}");
            return user.MagicToken.Token;
        }
    }
}
