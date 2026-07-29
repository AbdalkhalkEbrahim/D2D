using Application.Queries.AccountSettings;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.AccountSettingsFeature
{
    public class GetAllDesignsQueryHandler : IRequestHandler<GetAllProfilesQuery, Result<List<ProducersProfilesReponse>>>
    {
        private readonly D2DContext _context;

        public GetAllDesignsQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ProducersProfilesReponse>>> Handle(GetAllProfilesQuery request, CancellationToken cancellationToken)
        {
            var producers = await _context.Producers
              .Where(p => !p.IsDeleted && p.ProducerDesigns.Any(g => !g.IsDeleted))
              .Select(p => new ProducersProfilesReponse
              {
                  ProducerId = p.Id,
                  AnonName = p.AnonName,
                  ProfileImage = p.ProfileImageUrl,
                  NumberOfDesigns = p.ProducerDesigns.Count(g => !g.IsDeleted),
                  NumberOfCollaborations = p.ProducerCustomerOffers.Count(po => po.OfferStatus == OfferStatus.Completed)
              })
              .ToListAsync();

            return producers;


        }
    }
}
