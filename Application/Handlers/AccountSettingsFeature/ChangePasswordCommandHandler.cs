using Application.Commands.SettingsFeature;
using Application.Response;
using Domain.Entities.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers.AccountSettingsFeature
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;

        public ChangePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
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