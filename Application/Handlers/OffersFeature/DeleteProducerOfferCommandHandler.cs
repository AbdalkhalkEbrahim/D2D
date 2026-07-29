using Application.Commands.OffersFeature;
using Application.Response;
using Domain.Enums.Status;
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
    public class DeleteProducerOfferCommandHandler : IRequestHandler<DeleteProducerOfferCommand, Result>
    {
        private readonly D2DContext _context;
        public DeleteProducerOfferCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteProducerOfferCommand request, CancellationToken cancellationToken)
        {
            var deletedOffer = await _context.ProducerCustomerOffers.Where(o => o.ID == request.OfferId && o.OfferStatus == OfferStatus.OnHold)
                .ExecuteDeleteAsync(cancellationToken);
            if (deletedOffer == 0)
                return Result.Failure(Messages.NotFound.WithTarget("Offer"));
            return Result.Success();
        }
    }
}
