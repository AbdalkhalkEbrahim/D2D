using Application.Queries.OffersFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Handlers.OffersFeature
{
    public class GetProducerCustomerOfferByIdQueryHandler : IRequestHandler<GetProducerCustomerOfferByIdQuery, Result<ProducerOfferResponse>>
    {
        private readonly D2DContext _context;
        public GetProducerCustomerOfferByIdQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ProducerOfferResponse>> Handle(GetProducerCustomerOfferByIdQuery request, CancellationToken cancellationToken)
        {
            var offer =await _context.ProducerCustomerOffers.
                Include(po=>po.Producer).
                Select(po=>new {po.OfferStatus,po.ProducerID, po.Producer.AnonName , po.Producer.Reviews , po.Producer.Rate , po.Price , po.ID,po.CreatedAt,po.UpdatedAt, po.Diposit, po.Duration, po.CustomerPublishedOffer.Name, po.Steps, ImagesUrls = po.CustomerPublishedOffer.CustomerDesign.DesignImages.Select(im=>im.ImageUrl) })
                .FirstOrDefaultAsync(po => po.ID == request.OfferId &&po.OfferStatus != OfferStatus.Declined);

            if(offer == null)
                return Result<ProducerOfferResponse>.Failure(Messages.NotFound.WithTarget("Offer"));

            var Reviews = await _context.Reviews
            .AsNoTracking()
           .Where(r => r.ProducerID == offer.ProducerID)
           .Select(r => new
           {
               r.Customer.AnonName,
               r.Content,
               r.Rate
           })
           .ToDictionaryAsync(
               d => d.AnonName,
               d => new Tuple<string, int>(d.Content, d.Rate),
               cancellationToken
           );



            return new ProducerOfferResponse
            {
                CreatedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt,
                ProducerId = offer.ProducerID,
                OfferStatus=offer.OfferStatus.ToString(),
                ProducerOfferId = offer.ID,
                Rate = offer.Rate,
                Price = offer.Price,
                ProducerAnnonName = offer.AnonName,
                DeliveryTime = offer.Duration,
                Diposit = offer.Diposit,
                Name = offer.Name,
                Steps = offer.Steps.ToDictionary(s => s.StepName, s => new Tuple<int, int>(s.MinDuration, s.MaxDuration)),
                ImageUrl = offer.ImagesUrls.ToList(),
                Reviews = Reviews//_context.Customers.Select(c => new { c.AnonName, Review = c.Reviews.Select(r=>new {r.ProducerID, r.Content, r.Rate}).FirstOrDefault(r => r.ProducerID == offer.ProducerID) }).ToDictionary(d => d.AnonName, d => new Tuple<string,int>(d.Review.Content, d.Review.Rate))
            };
        }
    }
}
