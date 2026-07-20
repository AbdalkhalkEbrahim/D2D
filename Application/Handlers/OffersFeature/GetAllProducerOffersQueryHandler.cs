using Application.Queries.OffersFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
namespace Application.Handlers.OffersFeature
{
    public class GetAllProducerOffersQueryHandler : IRequestHandler<GetAllProducerOffersQuery, Result<List<ProducerOfferResponse>>>
    {
        private readonly D2DContext _context;

        public GetAllProducerOffersQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ProducerOfferResponse>>> Handle(GetAllProducerOffersQuery request, CancellationToken cancellationToken)
        {
            var Producer = _context.ProducerCustomerOffers.Select(po =>new {
                ProducerId = po.Producer.Id, po.ID, po.Producer.AnonName, po.CustomerPublishedOffer.CustomerDesign.Name,
                DesignImages = po.CustomerPublishedOffer.CustomerDesign.DesignImages.Select(im=>im.ImageUrl), po.Price, po.OfferStatus,
                po.CreatedAt, po.UpdatedAt,po.Producer.IsDeleted,
                Review = po.Producer.Reviews.Select(r=> new {po.Producer.AnonName, r.Content, r.Rate}), po.Producer.Rate,
                Steps = po.Steps.Select(s=> new {s.StepName, s.MinDuration, s.MaxDuration}),
                po.Diposit, po.Duration
            }).Where(p => p.ProducerId == request.ProducerId&&!p.IsDeleted);

            if (Producer.Count() == 0)
                return Result<List<ProducerOfferResponse>>.Failure(Messages.NotFound.WithTarget("User"));

            var counts = 

            Producer = Producer.OrderByDescending(p => p.CreatedAt);
            if (!request.Newest)
                Producer = Producer.Reverse();

            if (request.IsAccepted)
                Producer = Producer.Where(p => p.OfferStatus == OfferStatus.Accepted);
            else if (request.IsPending)
                Producer = Producer.Where(p => p.OfferStatus == OfferStatus.OnHold);
            else if (request.IsDecilned)
                Producer = Producer.Where(p => p.OfferStatus == OfferStatus.Declined);

            

            var response = new List<ProducerOfferResponse>();
            foreach(var offer in Producer)
            {
                response.Add(new ProducerOfferResponse
                {
                    ProducerId = offer.ProducerId,
                    ProducerOfferId = offer.ID,
                    ProducerAnnonName = offer.AnonName,
                    Name = offer.Name,
                    ImageUrl = offer.DesignImages.ToList(),
                    Price = offer.Price,
                    OfferStatus= offer.OfferStatus.ToString(),
                    CreatedAt = offer.CreatedAt,
                    UpdatedAt = offer.UpdatedAt,
                    Reviews = offer.Review.ToDictionary(u=>u.AnonName, c=> new Tuple<string, int>(c.Content,c.Rate)),
                    Rate = offer.Rate,
                    Steps = offer.Steps.ToDictionary(s=>s.StepName, d=> new Tuple<int, int>(d.MinDuration,d.MaxDuration)),
                    Diposit = offer.Diposit,
                    DeliveryTime = offer.Duration
                });
            }
            return response;

        }
    }
}
