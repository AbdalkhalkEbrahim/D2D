using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Admin
{
    public class GetProducerProfileQueryHandler : IRequestHandler<GetProducerProfileQuery, Result<GetUserProfileAdminResponse>>
    {
        private readonly D2DContext _context;
        public GetProducerProfileQueryHandler(D2DContext context)
        {
            _context = context;
        }

        public async Task<Result<GetUserProfileAdminResponse>> Handle(GetProducerProfileQuery request, CancellationToken cancellationToken)
        {
            var producer = await _context.Producers.Where(p => p.Id == request.id&&!p.IsDeleted).Select(p => new GetUserProfileAdminResponse
            {
                Id = p.Id,
                Name = p.FirstName + " " + p.LastName,
                Email = p.Email,
                ProfileImageUrl = p.ProfileImageUrl,
                ReportCount = p.NumOfReports,
                TotalCollaborations = p.NumOfCollaborations,
                Type = p.UserType.ToString(),
                JoinDate = p.JoinDate,
                Status = p.IdentityStatus.ToString(),
                AnonName = p.AnonName,
                FrontSideIdUrl = p.FrontImageID,
                BackSideIdUrl = p.BackImageID,
                PersonalImageUrl = p.PersonalImage,
                LicenseUrlForProducer = p.LicenseVerifications.Select(l => l.LicenseUrl).ToList()
            }).FirstOrDefaultAsync();

            if (producer == null)
                return Result<GetUserProfileAdminResponse>.Failure(Messages.NotFound.WithTarget("User"));


            return producer;
        }
    }
}
