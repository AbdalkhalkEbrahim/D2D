using Application.Queries.OffersFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            if((request.DesignId == null && request.PublishedOfferId == null) || (request.DesignId != null && request.PublishedOfferId != null))
                return Result<CustomerOfferResponse>.Failure(Messages.BadRequest.WithTarget("Default"));

            var offer = await _context.CustomerPublishedOffers.AsNoTracking()
                .Select(cpo=>new
                {
                    cpo.CustomerDesignID,
                    cpo.ID,
                    DesignImages = cpo.CustomerDesign.DesignImages.Select(di => di.ImageUrl),
                    cpo.Name,
                    cpo.Customer.Addresses.FirstOrDefault(add=>add.Selected).City,
                    cpo.Description,
                    cpo.Category,
                    cpo.Amount,
                    cpo.Duration,
                    cpo.Colors,
                    cpo.Gender,
                    cpo.Material,
                    cpo.MaxPrice,
                    cpo.PrintingType,
                    cpo.Sizes,
                    cpo.SizesFile,
                    cpo.TargetAudience,
                    cpo.IsActive,
                    cpo.CreatedAt,
                    cpo.UpdatedAt,
                    ProducersOffers = cpo.ProducerCustomerOffers.Select(pco => pco.ID),
                    cpo.Customer.IsDeleted,
                    cpo.CustomerID
                })
                .FirstOrDefaultAsync(o =>!o.IsDeleted&& (request.DesignId != null && request.DesignId == o.CustomerDesignID) ||
                (request.PublishedOfferId != null && o.ID == request.PublishedOfferId), cancellationToken);

            if (offer == null) 
                return Result<CustomerOfferResponse>.Failure(Messages.NotFound.WithTarget("Offer"));

            return new CustomerOfferResponse
            {
                PublishedOfferID = offer.ID,
                CustomerId=offer.CustomerID,
                DesignImages = offer.DesignImages.ToList(),
                Name = offer.Name,
                City = offer.City,
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
                ProducersOffersIDs = offer.ProducersOffers.ToList(),
            };

        }
    }
}
