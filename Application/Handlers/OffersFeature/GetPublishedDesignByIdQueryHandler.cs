using Application.Queries.OffersFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature
{
    public class GetPublishedDesignByIdQueryHandler : IRequestHandler<GetPublishedDesignByIdQuery, Result<CustomerOfferResponse>>
    {
        private readonly D2DContext _context;

        public GetPublishedDesignByIdQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<CustomerOfferResponse>> Handle(GetPublishedDesignByIdQuery request, CancellationToken cancellationToken)
        {
            var offer = await _context.CustomerPublishedOffers.AsNoTracking().Include(cpo=>cpo.ProducerCustomerOffers).Include(cpo=>cpo.CustomerDesign).Include(cpo=>cpo.CustomerDesign.DesignImages).Where(cpo=>cpo.ProducerCustomerOffers!=null).FirstOrDefaultAsync(o => o.ID == request.OfferId, cancellationToken);
            if (offer == null) 
                return Result<CustomerOfferResponse>.Failure(Messages.NotFound.WithTarget("Offer"));
            return new CustomerOfferResponse
            {
                ID = request.OfferId,
                DesignImages = offer.CustomerDesign.DesignImages.Select(di => di.ImageUrl).ToList(),
                Name = offer.Name,
                Category = offer.Category,
                Description = offer.Description,
                Amount = offer.Amount,
                Colors = offer.Colors,
                Duration = offer.Duration,
                Gender = !offer.Gender?"Male":"Female",
                Material = offer.Material,
                MaxPrice = offer.MaxPrice,
                PrintingType = offer.PrintingType,
                Sizes = offer.Sizes,
                SizesFile = offer.SizesFile,
                TargetAudience = offer.TargetAudience,
                IsActive = offer.IsActive,
                PublishedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt,
                ProducersOffersIDs=offer.ProducerCustomerOffers.Select(pco => pco.ID).ToList()
            };

        }
    }
}
