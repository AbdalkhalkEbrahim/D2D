using Application.Commands.SettingsFeature;
using Application.Response;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly D2DContext _context;

        public ChangePasswordCommandHandler(UserManager<User> userManager,D2DContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<Result<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted);
            if (user == null)
                return Result<string>.Failure(Messages.NotFound.WithTarget("User"));

            if (request.NewPassword != request.ConfirmPassword)
                return Result<string>.Failure(Messages.BadRequest.WithTarget("PasswordMismatch"));
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                return Result<string>.Failure(Messages.BadRequest.WithTarget("PasswordChangeFailed"));

            return Result<string>.Success("Password has been successfully updated.");
        }
    }
}