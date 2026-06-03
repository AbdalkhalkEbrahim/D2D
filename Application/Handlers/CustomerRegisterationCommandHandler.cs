using Application.Commands;
using Application.Services;
using Domain.DTOs;
using Domain.Entities.Customers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Handlers
{
    public class CustomerRegisterationCommandHandler:IRequestHandler<CustomerRegisterationCommand, JwtToken>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly IAuthService _authService;

        private readonly D2DContext _context;
        public CustomerRegisterationCommandHandler(UserManager<User> userManager, IUploadService uploadService, IAuthService authService, D2DContext context)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _authService = authService;
            _context = context;
        }
        public async Task<JwtToken>Handle(CustomerRegisterationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.CustomerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Customer)
                throw new Exception("Invalid request");

            Customer customer = (Customer) user;
            customer.Addresses = new HashSet<Address>
                {
                    new Address
                    {
                        AppartmentNo = request.AppartmentNo,
                        Street = request.Street,
                        City = request.City,
                        Goverate = request.Goverate,
                        Selected = true,
                    }
                };
            
            customer.PersonalImage = await _uploadService.UploadFileAsync(request.PersonalImage);
            customer.FrontImageID = await _uploadService.UploadFileAsync(request.FrontImageID);
            customer.BackImageID = await _uploadService.UploadFileAsync(request.BackImageID);

            //await _userManager.UpdateAsync(customer);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            var result = new Dictionary<string,string>
            {
                { "FrontImageID", customer.FrontImageID },
                { "BackImageID", customer.BackImageID },
                { "PersonalImage", customer.PersonalImage }
            };

            var AccessToken = await _authService.GenerateAccessToken(customer);
            var refreshToken = await _authService.GenerateRefreshToken(customer.Id);
            return new JwtToken
            {
                UserID = customer.Id,
                AccessToken = AccessToken.Token,
                RefreshToken = refreshToken.Token,
                AccessTokenExpiresAt = AccessToken.ExpiresAt,
                RefreshTokenExpiresAt = refreshToken.ExpiresAt,
                Data = result,
            };
        }
    }
}
