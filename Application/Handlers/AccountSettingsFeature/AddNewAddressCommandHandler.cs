using Application.Commands.AccountSettingsFeature;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.Entities.Customers;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class AddNewAddressCommandHandler : IRequestHandler<AddNewAddressCommand, Result<int>>
    {
        private readonly D2DContext _context;
        public AddNewAddressCommandHandler(D2DContext context)
        {
            _context=context;
        }
        public async Task<Result<int>> Handle(AddNewAddressCommand request, CancellationToken cancellationToken)
        {
            var user=await _context.Customers.FirstOrDefaultAsync(u => u.Id == request.CustomerId&& !u.IsDeleted);
            if(user==null)
                return Result<int>.Failure(Messages.NotFound.WithTarget("User"));
            user.Addresses.Add(new Address
            {
                AppartmentNo = request.AppartmentNo,
                BuildingNumber = request.BuildingNumber,
                Street = request.Street,
                District = request.District,
                City = request.City,
                Goverate = request.Goverate,
            });

            if(request.Selected)
               user.SelectSpecificAddress(user.Addresses.Last());

            _context.Customers.Update(user);
            await _context.SaveChangesAsync();


            return user.Addresses.Last().ID;
        }
    }
}
