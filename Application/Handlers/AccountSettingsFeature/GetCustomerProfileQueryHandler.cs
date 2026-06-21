using Application.Queries.AccountSettings;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class GetCustomerProfileQueryHandler : IRequestHandler<GetCustomerProfileQuery, Result<CustomerProfileResponse>>
    {
        private readonly D2DContext _context;
        public GetCustomerProfileQueryHandler(D2DContext context)
        {
            _context= context;
        }
        public async Task<Result<CustomerProfileResponse>> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Customers.AsNoTracking().Select(c => new {c.Id,c.FirstName,c.LastName,c.Email,c.PhoneNumber,c.BD,c.ProfileImageUrl,c.AnonName,c.Addresses}).FirstOrDefaultAsync(u => u.Id == request.CustomerId);
            if (user == null)
                return Result<CustomerProfileResponse>.Failure(Messages.NotFound.WithTarget("User"));
            var address = user.Addresses.FirstOrDefault(a => a.Selected);

            var profile = new ProfileResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AnonName = user.AnonName,
                BD = user.BD,
                ProfileImageUrl = user.ProfileImageUrl
            };
            var customerProfile = new CustomerProfileResponse
            {
                Profile = profile,
                Goverate = address.Goverate,
                Street = address.Street,
                City = address.City   
            };

            return Result<CustomerProfileResponse>.Success(customerProfile);
        }
    }
}
