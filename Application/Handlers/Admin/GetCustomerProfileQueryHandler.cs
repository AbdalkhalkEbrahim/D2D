using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Admin
{
    public class GetCustomerProfileQueryHandler : IRequestHandler<GetCustomerProfileQuery, Result<GetUserProfileAdminResponse>>
    {
        private readonly D2DContext _context;
        public GetCustomerProfileQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<GetUserProfileAdminResponse>> Handle(GetCustomerProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Customers.Where(u => u.Id == request.id).Select(u => new GetUserProfileAdminResponse
            {
                Id = u.Id,
                Name = u.FirstName + " " + u.LastName,
                Email = u.Email,
                ProfileImageUrl = u.ProfileImageUrl,
                ReportCount = u.NumOfReports,
                TotalCollaborations = u.NumOfCollaborations,
                Type = u.UserType.ToString(),
                JoinDate = u.JoinDate,
                Status = u.IdentityStatus.ToString(),
                AnonName = u.AnonName,
                FrontSideIdUrl = u.FrontImageID,
                BackSideIdUrl = u.BackImageID,
                PersonalImageUrl = u.PersonalImage,
                AddressForCustomer = u.Addresses.FirstOrDefault(a => a.Selected).City
            }).FirstOrDefaultAsync();

            if (user == null)
                return Result<GetUserProfileAdminResponse>.Failure(Messages.NotFound.WithTarget("USer"));

            return user;
        }
    }
}
