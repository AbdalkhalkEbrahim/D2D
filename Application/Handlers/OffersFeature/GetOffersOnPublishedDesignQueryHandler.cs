using Application.Queries.OffersFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Entities.Designs;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.OffersFeature
{
    public class GetOffersOnPublishedDesignQueryHandler : IRequestHandler<GetOffersOnPublishedDesignQuery, Result<List<ProducerOfferResponse>>>
    {
        private readonly D2DContext _context;
        public GetOffersOnPublishedDesignQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ProducerOfferResponse>>> Handle(GetOffersOnPublishedDesignQuery request, CancellationToken cancellationToken)
        {
            //var publishedDesignOffers = _context.ProducerCustomerOffers.Where(pdo => pdo.CustomerPublishedOffer != null && pdo.CustomerPublishedOffer.ID == request.PublishedDesignId && pdo.CustomerPublishedOffer.CustomerDesign.DesignImages.Select(di => di.CustomerDesignID).Contains(pdo.CustomerPublishedOffer.CustomerDesignID))
            //    .Select(pdo => new {pdo.CustomerPublishedOffer, pdo.Producer.Rate,pdo.CreatedAt,pdo.Price,pdo.Producer.AnonName })
            //    ;
             var publishedDesignOffers = _context.ProducerCustomerOffers
            .Where(pdo => pdo.CustomerPublishedOffer != null&& pdo.CustomerPublishedOfferID == request.PublishedDesignId && pdo.OfferStatus==OfferStatus.OnHold&& !pdo.Producer.IsDeleted)
            .Select(pdo => new
            {
                pdo.OfferStatus,
                pdo.ProducerID,
                pdo.CustomerPublishedOffer.Name,
                pdo.ID,
                pdo.CustomerPublishedOfferID,
                pdo.CustomerPublishedOffer.CustomerDesignID,
                pdo.CustomerPublishedOffer.MaxPrice,
                pdo.CreatedAt,
                pdo.UpdatedAt,
                pdo.Price,
                pdo.Diposit,
                pdo.Duration,
                pdo.Producer.AnonName,
                pdo.Producer.Rate,
                pdo.Steps,
                
                DesignImages = pdo.CustomerPublishedOffer.CustomerDesign.DesignImages
                    .Select(di => di.ImageUrl).ToList()
            });
            if(request.BestMatch)
            {
                publishedDesignOffers = publishedDesignOffers.OrderBy(pdo => Math.Abs(pdo.Price - pdo.MaxPrice)).ThenBy(pdo=>pdo.Price);
            }
            else if (request.LowestPrice)
            {
                publishedDesignOffers = publishedDesignOffers.OrderBy(pdo => pdo.Price);
            }
            else if (request.NewestOffer)
            {
                publishedDesignOffers = publishedDesignOffers.OrderByDescending(pdo => pdo.CreatedAt);
            }
            else if (request.HighestRate)
            {
                publishedDesignOffers = publishedDesignOffers.OrderByDescending(pdo => pdo.Rate);
            }

            if(!await publishedDesignOffers.AnyAsync())
                return Result<List<ProducerOfferResponse>>.Failure(Messages.NotFound.WithTarget("Offer"));


            if (request.PageNum.HasValue && request.PageNum.Value > 0)
                publishedDesignOffers = publishedDesignOffers.Skip(((int)request.PageNum - 1) * (int)request.PageSize).Take((int)request.PageSize);


            var result= new List<ProducerOfferResponse>();
            foreach (var item in publishedDesignOffers.ToList())
            {
                result.Add
                    (
                    new ProducerOfferResponse
                    {
                        CreatedAt = item.CreatedAt,
                        UpdatedAt = item.UpdatedAt,
                        ImageUrl = item.DesignImages,
                        ProducerOfferId = item.ID,
                        Rate = item.Rate,
                        Price = item.Price,
                        Name = item.Name,
                        ProducerAnnonName = item.AnonName,
                        ProducerId = item.ProducerID,
                        OfferStatus = item.OfferStatus.ToString(),
                        DeliveryTime = item.Duration,
                        Diposit = item.Diposit,
                        Steps = item.Steps.ToDictionary(s => s.StepName, s => new Tuple<int, int>(s.MinDuration, s.MaxDuration)),
                    }
                    );
            }
            return result;
        }
    }
}
