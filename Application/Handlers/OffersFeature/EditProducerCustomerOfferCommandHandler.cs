using Application.Commands.OffersFeature;
using Application.Response;
using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using jsonPatch = Microsoft.AspNetCore.JsonPatch.Operations;

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
           var producerOffer= await _context.ProducerCustomerOffers.Include(pco=>pco.CustomerPublishedOffer)
                .FirstOrDefaultAsync(po=>po.ID == request.OfferId && po.ProducerID == request.ProducerId && (po.OfferStatus == OfferStatus.OnHold ||(po.OfferStatus == OfferStatus.Accepted && po.CustomerPublishedOffer.ActiveOfferLogs.Count() == 1)  ));
            if (producerOffer == null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("Offer"));

            var entityPatch = new JsonPatchDocument<ProducerCustomerOffer>();
            request.data.Operations.ForEach(op => entityPatch.Operations.Add(new jsonPatch.Operation<ProducerCustomerOffer>(op.op, op.path, op.from, op.value)));
            // Console.WriteLine(request.data.Operations.Count);
            entityPatch.ApplyTo(producerOffer);

            /*  producerOffer.Price = request.Price;
              producerOffer.Duration = request.DeliveryTime;
              producerOffer.Diposit = request.Diposit;
              producerOffer.Steps = request.Steps.Select(s => new ProducerSteps
              {
                  StepName = s.Key,
                  MinDuration = s.Value.Item1,
                  MaxDuration = s.Value.Item2
              }).ToList();
              producerOffer.UpdatedAt = DateTime.UtcNow;
              _context.Update(producerOffer);*/
            await _context.SaveChangesAsync();
            return producerOffer.ID;
        }
    }
}
