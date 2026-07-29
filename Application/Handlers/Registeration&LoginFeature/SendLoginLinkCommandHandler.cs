using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers
{
    public class SendLoginLinkCommandHandler : IRequestHandler<SendLoginLinkCommand, Result<string>>
    {
        private readonly D2DContext _context;
        private readonly IEmailService _emailService;

        public SendLoginLinkCommandHandler(D2DContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<Result<string>> Handle(SendLoginLinkCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.MagicToken)
                .FirstOrDefaultAsync(u => u.Id == request.UserID&& !u.IsDeleted, cancellationToken);

            if (user is null)
                return Result<string>.Failure(Messages.NotFound.WithTarget("User"));

            string tokenString = Guid.NewGuid().ToString("N");

            if (user.MagicToken is not null)
            {
                _context.Remove(user.MagicToken);//
            }
            user.MagicToken = new MagicToken
            {
                Token = tokenString,
                Expiration = DateTime.UtcNow.AddHours(1),
                IsUsed = false
            };

            user.IdentityStatus = VerificationStatus.Approved;

            await _context.SaveChangesAsync(cancellationToken);

            string link = $"https://design-to-dress.vercel.app/verify-magic-token?token={tokenString}";

            BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(user.Email!, "Your Login Link", $"Click the link to login: {link}"));

            return Result<string>.Success(tokenString);
        }
    }
}