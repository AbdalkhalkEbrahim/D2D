using Application.Queries.AccountSettings;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class GetAddressByIdQueryHandler : IRequestHandler<GetAddressByIdQuery, Result<AddressResponse>>
    {
        private readonly D2DContext _context;
        public GetAddressByIdQueryHandler(D2DContext context)
        {
            _context = context;   
        }

        public async Task<Result<AddressResponse>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            //var user=await _context.Customers.FirstOrDefaultAsync(c=>c.Addresses.Any(a=>a.ID==request.AddressId));
            //if (user == null)
            //    return Result<AddressResponse>.Failure(Messages.NotFound.WithTarget("Address"));

            //var address = user.Addresses.FirstOrDefault(a => a.ID == request.AddressId);
            var address=_context.Customers.AsNoTracking().SelectMany(c => c.Addresses).FirstOrDefault(a => a.ID == request.AddressId);

            if (address == null)
                return Result<AddressResponse>.Failure(Messages.NotFound.WithTarget("Address"));

            return Result<AddressResponse>.Success(new AddressResponse
            {
                ID = address.ID,
                CustomerId = address.CustomerID,
                AppartmentNo = address.AppartmentNo,
                BuildingNumber = address.BuildingNumber,
                Street = address.Street,
                District = address.District,
                City = address.City,
                Goverate = address.Goverate,
                Selected = address.Selected
            });
        }
    }
}
