using Application.Commands;
using Application.Commands.RegisterationFeature;
using Application.Response;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;
        private readonly D2DContext _context;
        public ForgetPasswordCommandHandler(UserManager<User> userManager, IMediator mediator, D2DContext context)
        {
            _userManager = userManager;
            _mediator = mediator;
            _context = context;
        }

        public async Task<Result<string>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
/*            var verification = await _mediator.Send(new VerifyOtpCommand { Otp =  request.Otp , UserId = request.Id});
            if(!verification.IsSuccess)
                return Result<string>.Failure(verification.Error);
*/
            if (request.NewPassword != request.ConfirmPassword)
                return Result<string>.Failure(Messages.BadRequest.WithTarget("PasswordMismatch"));

            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id == request.Id);
            if (user == null)
                return Result<string>.Failure(Messages.NotFound.WithTarget("User"));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);

            if (!result.Succeeded)
                return Result<string>.Failure(Messages.BadRequest.WithTarget("PasswordChangeFailed"));

            return Result<string>.Success("Password has been successfully updated.");
        }
    }
}