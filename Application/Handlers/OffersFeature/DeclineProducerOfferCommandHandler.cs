using Application.Commands.OffersFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Offers;
using Domain.Enums.Status;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature
{
    public class DeclineProducerOfferCommandHandler : IRequestHandler<DeclineProducerOfferCommand, Result>
    {
        private readonly D2DContext _context;
        public DeclineProducerOfferCommandHandler(D2DContext context)
        {
            _context= context;
        }
        public async Task<Result> Handle(DeclineProducerOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _context.ProducerCustomerOffers.Select(o => new {o.ID,o.ProducerID,o.OfferStatus,o.UpdatedAt, o.CustomerPublishedOffer.Name}).FirstOrDefaultAsync(o => o.ID == request.OfferId);
            if (offer == null)
                return Result.Failure(Messages.NotFound.WithTarget("Offer"));
            var producerCustomerOffer = new ProducerCustomerOffer { ID=offer.ID ,OfferStatus = OfferStatus.Declined, UpdatedAt = DateTime.UtcNow };
            _context.Attach(producerCustomerOffer);
            _context.Entry(producerCustomerOffer).Property(o => o.OfferStatus).IsModified = true;
            _context.Entry(producerCustomerOffer).Property(o => o.UpdatedAt).IsModified = true;
            await _context.SaveChangesAsync();
            BackgroundJob.Enqueue<INotificationService>(notificationService => notificationService.DeclineProducerOfferNotification(offer.ProducerID,offer.Name));
            return Result.Success();
        }
    }
}
