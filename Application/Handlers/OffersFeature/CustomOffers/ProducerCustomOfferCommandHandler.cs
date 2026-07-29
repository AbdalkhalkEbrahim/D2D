using Application.Commands.OffersFeature;
using Application.Commands.OffersFeature.CustomOffer;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Entities.Offers;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class ProducerCustomOfferCommandHandler : IRequestHandler<ProducerCustomOfferCommand, Result<ProducerOfferResponse>>
    {
        private readonly D2DContext _context;
        public ProducerCustomOfferCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ProducerOfferResponse>> Handle(ProducerCustomOfferCommand request, CancellationToken cancellationToken)
        {
            var customOffer = await _context.CustomerCustomOffers.Select(cco => new { cco.ProducerDesign, cco.ID, cco.ProducerCustomerOfferID })
                .FirstOrDefaultAsync(cco => cco.ID == request.CustomerCustomOfferId && cco.ProducerDesign.ProducerID == request.ProducerId);

            if (customOffer == null)
                return Result<ProducerOfferResponse>.Failure(Messages.NotFound.WithTarget("User"));
            // if (offer != null || (offer.Producer.UserType != UserType.Producer))
           

            var producerOffer = new ProducerCustomerOffer
            {
                ProducerID = request.ProducerId,
                Price = request.Price,
                Duration = request.DeliveryTime,
                Diposit = request.Diposit,
                CustomOfferId=customOffer.ID,
                Steps = request.Steps.Select(s => new ProducerSteps
                {
                    StepName = s.Key,
                    MinDuration = s.Value.Item1,
                    MaxDuration = s.Value.Item2
                }).ToList()
            };

            _context.Add(producerOffer);
            await _context.SaveChangesAsync();

            var raws = await _context.CustomerCustomOffers.Where(cco=>cco.ID == request.CustomerCustomOfferId)
                .ExecuteUpdateAsync(property => property.SetProperty(p => p.ProducerCustomerOfferID, p => producerOffer.ID));

            if (raws == 0)
                return Result<ProducerOfferResponse>.Failure(new Error("BadRequest", "Can't reply to this offer"));

            BackgroundJob.Enqueue<INotificationService>(notificationService => notificationService.SendProducerOfferNotification(request.CustomerId, producerOffer.ID));


            var producerInfo = await _context.Producers.Include(p => p.Reviews)
                .Where(p => p.Id == request.ProducerId)
                .Select(p => new {
                    p.Id,
                    p.AnonName,
                    p.Rate,
                    Reviews = p.Reviews.Select(r => new {
                        r.Content,
                        CustomerAnnonName = _context.Customers.Where(c => c.Id == r.CustomerID).Select(c => c.AnonName).FirstOrDefault()
                    }).ToList()
                }).FirstOrDefaultAsync();

            //var reviews = producerInfo.Reviews.ToDictionary(r => r.CustomerAnnonName, r => r.Content);
            var Reviews = await _context.Reviews
            .AsNoTracking()
            .Where(r => r.ProducerID == producerInfo.Id)
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
            return new ProducerOfferResponse
            {
                ProducerId = producerInfo.Id,
                ProducerOfferId = producerOffer.ID,
                Rate = producerInfo.Rate,
                ProducerAnnonName = producerInfo.AnonName,
                OfferStatus = producerOffer.OfferStatus.ToString(),
                Price = request.Price,
                Reviews = Reviews,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
