using Application.Commands;
using Domain.DTOs;
using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class UserRegisterationCommandHandler : IRequestHandler<UserRegisterationCommand,string>
    {
        private readonly UserManager<User> _userManager;
        private readonly IOtpService _otpService;
        private readonly D2DContext _context;
        private readonly IEmailService _emailService;

        public UserRegisterationCommandHandler(UserManager<User> userManager, IOtpService otpService, D2DContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _otpService = otpService;
            _context = context;
            _emailService = emailService;
        }

        public async Task<string> Handle(UserRegisterationCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ComfirmedPassword)
                throw new Exception("Passwords do not match.");

            var existingEmail =await _userManager.FindByEmailAsync(request.Email);

            if (existingEmail != null)
                throw new Exception("Email already exists.");

            var user = new User
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserType = request.UserType,
                BD = new DateTime(request.Year, request.Month, request.Day)
            };
            var result=await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                throw new Exception("aaaaaaaaaaaaaaaaaaaaaaaaaaaah");

            await _userManager.AddToRoleAsync(user, request.UserType.ToString());

            var code = _otpService.GenerateOtp();

            var otp = new Otp
            {
                Code = code,
                UserId = user.Id,
                ExpirationTime = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };
            
            
           
            await _context.Otps.AddAsync(otp, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _emailService.SendEmailAsync(request.Email, "OTP Email Verification", $"Your OTP is: {code}");
            
            return request.UserType.ToString();
        }

    }
}
