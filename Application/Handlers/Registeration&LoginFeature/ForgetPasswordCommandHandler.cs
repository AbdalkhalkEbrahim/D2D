using Application.Commands;
using Application.Commands.RegisterationFeature;
using Application.Response;
using Domain.Entities.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public ForgetPasswordCommandHandler(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Result<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.NewPassword != request.ConfirmPassword)
                return Result<string>.Failure(Messages.BadRequest.WithTarget("PasswordMismatch"));

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return Result<string>.Failure(Messages.NotFound.WithTarget("User"));

            var verifyOtp = await _mediator.Send(new VerifyOtpCommand { UserId = user.Id, Otp = request.Otp });
            if (verifyOtp.IsSuccess==false)
                return Result<string>.Failure(Messages.Expired.WithTarget("Otp"));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
                return Result<string>.Failure(Messages.BadRequest.WithTarget("PasswordChangeFailed"));

            return Result<string>.Success("Password has been successfully updated.");
        }
    }
}