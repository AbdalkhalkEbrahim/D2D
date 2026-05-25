using Application.Commands;
using Application.Services;
using Domain.Entities.Customers;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CustomerRegisterationCommandHandler:IRequestHandler<CustomerRegisterationCommand, Dictionary<string, string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUploadService _uploadService;
        private readonly D2DContext _context;
        public CustomerRegisterationCommandHandler(UserManager<User> userManager, IUploadService uploadService, D2DContext context)
        {
            _userManager = userManager;
            _uploadService = uploadService;
            _context = context;
        }
        public async Task<Dictionary<string, string>> Handle(CustomerRegisterationCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.CustomerId);
            if (user == null || !user.EmailConfirmed || user.UserType != UserType.Customer)
                throw new Exception("Invalid request");

            var customer = new Customer
            {

                Addresses = new HashSet<Address>
                {
                    new Address
                    {
                        AppartmentNo = request.AppartmentNo,
                        Street = request.Street,
                        City = request.City,
                        Goverate = request.Goverate
                    }
                },

                FrontImageID = await _uploadService.UploadFileAsync(request.FrontImageID),
                BackImageID = await _uploadService.UploadFileAsync(request.BackImageID),
                PersonalImage = await _uploadService.UploadFileAsync(request.PersonalImage)
            };
          /*  await _userManager.UpdateAsync(customer);
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();*/
            var result = new Dictionary<string,string>
            {
                { "FrontImageID", customer.FrontImageID },
                { "BackImageID", customer.BackImageID },
                { "PersonalImage", customer.PersonalImage }
            };
            
            return result;
        }
    }
}
