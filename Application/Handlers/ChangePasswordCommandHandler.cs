using Application.Commands;
using Domain.Entities.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, string>
    {
        private readonly UserManager<User> _userManager;
        public ChangePasswordCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
         }
        public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found");

            if (request.NewPassword != request.ConfirmPassword)
                throw new Exception("Passwords do not match");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                throw new Exception("Failed to change password");

            return "Password has been successfully updated.";
        }
    }
}
