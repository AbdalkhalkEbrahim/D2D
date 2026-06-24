using Application.Queries.AccountSettings;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Result<ProfileResponse>>
    {
        private readonly D2DContext _context;
        public GetProfileQueryHandler(D2DContext context) { _context = context; }
        public async Task<Result<ProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
           var user =await _context.Users.Select(u => new {u.Id,u.FirstName,u.LastName,u.Email,u.PhoneNumber,u.AnonName,u.BD,u.ProfileImageUrl}).FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
                return Result<ProfileResponse>.Failure(Messages.NotFound.WithTarget("User"));

            var profile = new ProfileResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AnonName = user.AnonName,
                BD=user.BD,
                ProfileImageUrl = user.ProfileImageUrl
            };
            /*List of designs bought from designer*/
            return Result<ProfileResponse>.Success(profile);
        }
    }
}
