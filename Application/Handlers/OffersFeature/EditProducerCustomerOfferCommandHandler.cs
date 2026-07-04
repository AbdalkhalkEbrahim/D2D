using Application.Commands.OffersFeature;
using Application.Response;
using Domain.Entities.Offers;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.OffersFeature
{
    public class EditProducerCustomerOfferCommandHandler : IRequestHandler<EditProducerCustomerOfferCommand, Result<Guid>>
    {
        private readonly D2DContext _context;
        public EditProducerCustomerOfferCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> Handle(EditProducerCustomerOfferCommand request, CancellationToken cancellationToken)
        {
           var producerOffer= await _context.ProducerCustomerOffers.FirstOrDefaultAsync(po=>po.ID == request.OfferId && po.ProducerID == request.ProducerId && po.OfferStatus == OfferStatus.OnHold);
            if (producerOffer == null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("Offer"));

            producerOffer.Price = request.Price;
            producerOffer.Duration = request.DeliveryTime;
            producerOffer.Diposit = request.Diposit;
            producerOffer.Steps = request.Steps.Select(s => new ProducerSteps
            {
                StepName = s.Key,
                MinDuration = s.Value.Item1,
                MaxDuration = s.Value.Item2
            }).ToList();
            producerOffer.UpdatedAt = DateTime.UtcNow;
            _context.Update(producerOffer);
            await _context.SaveChangesAsync();
            return producerOffer.ID;
        }
    }
}
