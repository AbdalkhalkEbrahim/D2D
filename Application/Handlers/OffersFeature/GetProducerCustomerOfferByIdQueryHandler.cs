using Application.Queries.OffersFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var offer =await _context.ProducerCustomerOffers.Include(po=>po.Producer).Select(po=>new {po.OfferStatus,po.ProducerID, po.Producer.AnonName , po.Producer.Reviews , po.Producer.Rate , po.Price , po.ID,po.CreatedAt,po.UpdatedAt }).FirstOrDefaultAsync(po => po.ID == request.OfferId);
            if(offer == null)
                return Result<ProducerOfferResponse>.Failure(Messages.NotFound.WithTarget("Offer"));
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
                Reviews = offer.Reviews.ToDictionary(r => _context.Customers.Where(c => c.Id == r.CustomerID).Select(c => c.AnonName).FirstOrDefault(), r => r.Content)//
            };
        }
    }
}
