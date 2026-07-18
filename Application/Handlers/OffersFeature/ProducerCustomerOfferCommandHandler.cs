using Application.Commands.OffersFeature;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Entities.Offers;
using Domain.Entities.Shared;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Handlers.OffersFeature
{
    public class ProducerCustomerOfferCommandHandler : IRequestHandler<ProducerCustomerOfferCommand,Result<ProducerOfferResponse>>
    {
        private readonly D2DContext _context;

        public ProducerCustomerOfferCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ProducerOfferResponse>> Handle(ProducerCustomerOfferCommand request, CancellationToken cancellationToken)
        {
           // var offer = await _context.ProducerCustomerOffers.AsNoTracking().Include(pco=>pco.Producer).FirstOrDefaultAsync(pco => pco.CustomerPublishedOfferID == request.CustomerPublishedOfferId && pco.ProducerID == request.ProducerId );

            var producer =await  _context.Producers.AsNoTracking().Include(p => p.ProducerCustomerOffers).Where(p => p.Id == request.ProducerId).Select(p=>p.ProducerCustomerOffers).FirstOrDefaultAsync();
            if(producer == null) 
                return Result<ProducerOfferResponse>.Failure(Messages.NotFound.WithTarget("User"));
            // if (offer != null || (offer.Producer.UserType != UserType.Producer))
            if (producer.Any(pco=>pco.CustomerPublishedOfferID == request.CustomerPublishedOfferId))
                return Result<ProducerOfferResponse>.Failure(Messages.Conflict.WithTarget("CustomerOffer"));

            var producerOffer = new ProducerCustomerOffer
            {
                ProducerID = request.ProducerId,
                CustomerPublishedOfferID = request.CustomerPublishedOfferId,
                Price = request.Price,
                Duration = request.DeliveryTime,
                Diposit = request.Diposit,
                Steps = request.Steps.Select(s => new ProducerSteps
                {
                    StepName = s.Key,
                    MinDuration = s.Value.Item1,
                    MaxDuration = s.Value.Item2
                }).ToList()
            };

            _context.Add(producerOffer);
            await _context.SaveChangesAsync();
            BackgroundJob.Enqueue<INotificationService>(notificationService => notificationService.SendProducerOfferNotification(request.CustomerId,producerOffer.ID));


            var producerInfo = await _context.Producers.Include(p => p.Reviews)
                .Where(p => p.Id == request.ProducerId)
                .Select(p => new {
                    p.Id,
                    p.AnonName,
                    p.Rate,
                    Reviews = p.Reviews.Select(r => new { 
                        r.Content,
                        CustomerAnnonName = _context.Customers.Where(c => c.Id == r.CustomerID).Select(c=>c.AnonName).FirstOrDefault() }).ToList()
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
                d => new Tuple<string,int>(d.Content, d.Rate), 
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
