using Application.Commands.RegisterationFeature;
using Application.Response;
using Domain.DTOs.RegisterationDtos;
using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class UserRegisterationCommandHandler : IRequestHandler<UserRegisterationCommand, Result<UserRegisterationResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;

        public UserRegisterationCommandHandler(UserManager<User> userManager, IMediator mediator)
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Result<UserRegisterationResponse>> Handle(UserRegisterationCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ComfirmedPassword)
                return Result<UserRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("PasswordMismatch"));

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
                return Result<UserRegisterationResponse>.Failure(Messages.Conflict.WithTarget("Email"));

            User user = request.UserType switch
            {
                UserType.Customer => new Customer(),
                UserType.Designer => new Designer(),
                _ => new Producer()
            };

            user.Email = request.Email;
            user.UserName = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserType = request.UserType;
            user.BD = new DateTime(request.Year, request.Month, request.Day);
            user.AnonName = user.AnonymousName(request.UserType);

            if (!user.IsAllowed)
                return Result<UserRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("Underage"));

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Result<UserRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("UserCreationFailed"));

            await _userManager.AddToRoleAsync(user, request.UserType.ToString());
            await _mediator.Send(new SendOtpCommand { Email = request.Email });

            return Result<UserRegisterationResponse>.Success(new UserRegisterationResponse {UserId=user.Id,Email=user.Email,UserType=user.UserType});
        }
    }
}