using Application.Queries.OffersFeature;
using Application.Queries.OffersFeature.CustomOffers;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class GetCustomRequestByIdQueryHandler : IRequestHandler<GetCustomRequestByIdQuery, Result<CustomerOfferResponse>>
    {
        private readonly D2DContext _context;

        public GetCustomRequestByIdQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<CustomerOfferResponse>> Handle(GetCustomRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var offer = await _context.CustomerCustomOffers
                .Select(cpo => new
                {
                    cpo.ProducerDesignID,
                    cpo.ID,
                    DesignImages = cpo.ProducerDesign.DesignImages.Select(di => di.ImageUrl),
                    cpo.Name,
                    cpo.Customer.Addresses.FirstOrDefault(add => add.Selected).City,
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
                    cpo.Customer.IsDeleted,
                    cpo.CustomerID
                }).AsNoTracking()
                .FirstOrDefaultAsync(o => !o.IsDeleted && !o.IsActive && o.ID == request.OfferId, cancellationToken);

            if (offer == null)
                return Result<CustomerOfferResponse>.Failure(Messages.NotFound.WithTarget("Offer"));

            return new CustomerOfferResponse
            {
                PublishedOfferID = offer.ID,
                CustomerId = offer.CustomerID,
                DesignImages = offer.DesignImages.ToList(),
                Name = offer.Name,
                City = offer.City,
                Category = offer.Category,
                Description = offer.Description,
                Amount = offer.Amount,
                Colors = offer.Colors,
                Duration = offer.Duration,
                Gender = !offer.Gender ? "Male" : "Female",
                Material = offer.Material,
                MaxPrice = offer.MaxPrice,
                PrintingType = offer.PrintingType,
                Sizes = offer.Sizes,
                SizesFile = offer.SizesFile,
                TargetAudience = offer.TargetAudience,
                IsActive = offer.IsActive,
                PublishedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt,
            };

        }
    }
}
