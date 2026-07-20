using Application.Queries.OffersFeature;
using Application.Queries.OffersFeature.CustomOffers;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class GetProducerCustomOfferByIdQueryHandler : IRequestHandler<GetProducerCustomOfferByIdQuery, Result<ProducerOfferResponse>>
    {
        private readonly D2DContext _context;
        public GetProducerCustomOfferByIdQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ProducerOfferResponse>> Handle(GetProducerCustomOfferByIdQuery request, CancellationToken cancellationToken)
        {
            var offer = await _context.ProducerCustomerOffers.
                Select(po => new {
                    po.OfferStatus,
                    po.ProducerID,
                    po.Producer.AnonName,
                    po.Producer.Reviews,
                    po.Producer.Rate,
                    po.Price,
                    po.ID,
                    po.CreatedAt,
                    po.UpdatedAt,
                    po.Diposit,
                    po.Duration,
                    Name = po.CustomerCustomOffer.Select(cco => cco.Name).FirstOrDefault(),
                    po.Steps,
                    ImagesUrls = po.CustomerCustomOffer.SelectMany(cco => cco.ProducerDesign.DesignImages.Select(im => im.ImageUrl)),
                    po.Producer.IsDeleted
                })
                .FirstOrDefaultAsync(po => po.ID == request.ProducerOfferId && po.OfferStatus != OfferStatus.Declined && !po.IsDeleted);

            if (offer == null)
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
               d => (d.Content, d.Rate),
               cancellationToken
           );

            var galleryList = await _context.ProducerDesigns
            .Where(g => g.ProducerID == offer.ProducerID && !g.IsDeleted)
            .Select(g => new
            {
                g.ID,
                Name = g.Name ?? string.Empty,
                Images = g.DesignImages.Select(im => im.ImageUrl).ToList()
            })
            .ToListAsync();

            Dictionary<(Guid Id, string Name), List<string>> gallery = galleryList
                .ToDictionary(
                    g => (g.ID, g.Name),
                    g => g.Images
                );


            return new ProducerOfferResponse
            {
                CreatedAt = offer.CreatedAt,
                UpdatedAt = offer.UpdatedAt,
                ProducerId = offer.ProducerID,
                OfferStatus = offer.OfferStatus.ToString(),
                ProducerOfferId = offer.ID,
                Rate = offer.Rate,
                Price = offer.Price,
                ProducerAnnonName = offer.AnonName,
                DeliveryTime = offer.Duration,
                Diposit = offer.Diposit,
                Name = offer.Name,
                Steps = offer.Steps.ToDictionary(s => s.StepName, s => (s.MinDuration, s.MaxDuration)),
                ImageUrl = offer.ImagesUrls.ToList(),
                Gallery = gallery,
                Reviews = Reviews//_context.Customers.Select(c => new { c.AnonName, Review = c.Reviews.Select(r=>new {r.ProducerID, r.Content, r.Rate}).FirstOrDefault(r => r.ProducerID == offer.ProducerID) }).ToDictionary(d => d.AnonName, d => new Tuple<string,int>(d.Review.Content, d.Review.Rate))
            };
        }
    }


}
