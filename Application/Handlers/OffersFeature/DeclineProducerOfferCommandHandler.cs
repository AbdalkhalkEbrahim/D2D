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
            var offer = await _context.ProducerCustomerOffers.Where(o => o.ID == request.OfferId && o.OfferStatus== OfferStatus.OnHold).ExecuteDeleteAsync(cancellationToken);
            if (offer == 0)
                return Result.Failure(Messages.NotFound.WithTarget("Offer"));
            return Result.Success();
        }
    }
}
