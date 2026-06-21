using Application.Queries.AccountSettings;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Handlers.AccountSettingsFeature
{
    public class GetCustomerAdrressesQueryHandler : IRequestHandler<GetCustomerAdrressesQuery, Result<List<AddressResponse>>>
    {
        private readonly D2DContext _context;
        public GetCustomerAdrressesQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<AddressResponse>>> Handle(GetCustomerAdrressesQuery request, CancellationToken cancellationToken)
        {
           var customer=await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CustomerId);
            if (customer == null)
                return Result<List<AddressResponse>>.Failure(Messages.NotFound.WithTarget("User"));

            var addresses = customer.Addresses.Select(a => new AddressResponse
            {
                ID = a.ID,
                AppartmentNo = a.AppartmentNo,
                BuildingNumber = a.BuildingNumber,
                Street = a.Street,
                District = a.District,
                City = a.City,
                Goverate = a.Goverate,
                Selected = a.Selected
            }).ToList();

            return Result<List<AddressResponse>>.Success(addresses);
        }
    }
}
