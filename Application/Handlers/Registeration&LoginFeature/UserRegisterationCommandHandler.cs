using Application.Commands.RegisterationFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.RegisterationDtos;
using Domain.Entities.Customers;
using d= Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers
{
    public class UserRegisterationCommandHandler : IRequestHandler<UserRegisterationCommand, Result<UserRegisterationResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMediator _mediator;
        private readonly D2DContext _context;
        private readonly IAuthService _authService;
        public UserRegisterationCommandHandler(UserManager<User> userManager, IMediator mediator, D2DContext context, IAuthService authService)
        {
            _userManager = userManager;
            _mediator = mediator;
            _context = context;
            _authService = authService;
        }

        public async Task<Result<UserRegisterationResponse>> Handle(UserRegisterationCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ComfirmedPassword)
                return Result<UserRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("PasswordMismatch"));

            var existingEmail = await _context.Users.FirstOrDefaultAsync(u=>u.Email == request.Email);
            if (existingEmail != null)
                return Result<UserRegisterationResponse>.Failure(Messages.Conflict.WithTarget("Email"));

            User user = request.UserType switch
            {
                UserType.Customer => new Customer(),
                UserType.Designer => new d.Designer(),
                _ => new Producer()
            };

            user.Email = request.Email;
            user.UserName = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserType = request.UserType;
            user.BD = new DateTime(request.Year, request.Month, request.Day);

            user.AnonName = await _authService.AnonymousName(request.UserType);

            if (!user.IsAllowed)
                return Result<UserRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("Underage"));

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, request.Password);
            /*            var result = await _userManager.CreateAsync(user, request.Password);
                        if (!result.Succeeded)
                            return Result<UserRegisterationResponse>.Failure(Messages.BadRequest.WithTarget("UserCreationFailed"));*/
            _context.Users.Add(user);
            _context.UserRoles.Add(new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = request.UserType == UserType.Customer ? "2" : (request.UserType == UserType.Producer ? "3" : "4")
            });
            //await _userManager.AddToRoleAsync(user, request.UserType.ToString());
            // await _mediator.Send(new SendOtpCommand { ID = user.Id });
            await _context.SaveChangesAsync(cancellationToken);
            return Result<UserRegisterationResponse>.Success(new UserRegisterationResponse {UserId=user.Id,Email=user.Email,UserType=user.UserType});
        }
    }
}