using Application.Commands.AccountSettingsFeature;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.Entities.Customers;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using jsonPatch = Microsoft.AspNetCore.JsonPatch.Operations;

namespace Application.Handlers.AccountSettingsFeature
{
    public class EditAddressCommandHandler : IRequestHandler<EditAddressCommand, Result<AddressResponse>>
    {
        private readonly D2DContext _context;

        public EditAddressCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<AddressResponse>> Handle(EditAddressCommand request, CancellationToken cancellationToken)
        {
            var addresses = _context.Customers.AsNoTracking().Where(c=>c.Id == request.CustomerId&&!c.IsDeleted).SelectMany(c => c.Addresses);

            if(!addresses.Any())
                return Result<AddressResponse>.Failure(Messages.NotFound.WithTarget("Address"));

            var updatedAddress = addresses.FirstOrDefault(a => a.ID == request.AddressId);
            if (updatedAddress == null)
                return Result<AddressResponse>.Failure(Messages.NotFound.WithTarget("Default"));

            var entityPatch = new JsonPatchDocument<Address>();
            request.data.Operations.ForEach(op => entityPatch.Operations
            .Add(new jsonPatch.Operation<Address>(op.op, op.path, op.from, op.value)));
            entityPatch.ApplyTo(updatedAddress);


            var exsistCustomer = new Customer { Id = addresses.First().CustomerID, Addresses = addresses.ToHashSet() };
            _context.Attach(exsistCustomer);

            if (request.Selected)
            {
                foreach (var address in exsistCustomer.Addresses)
                {
                    if (address.ID != request.AddressId)
                        address.Selected = false;
                    else
                        address.Selected = true;
                    _context.Entry(address).Property(c => c.Selected).IsModified = true;
                }
            }

            await _context.SaveChangesAsync();

            return new AddressResponse
            {
                ID = updatedAddress.ID,
                AppartmentNo = updatedAddress.AppartmentNo,
                BuildingNumber = updatedAddress.BuildingNumber,
                Street = updatedAddress.Street,
                District = updatedAddress.District,
                City = updatedAddress.City,
                Goverate = updatedAddress.Goverate,
                Selected = updatedAddress.Selected
            };
        }
    }
}
