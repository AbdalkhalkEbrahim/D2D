using Application.Commands.OffersFeature.CustomOffer;
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

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class DeclineCustomerOfferCommandHandler : IRequestHandler<DeclineCustomerOfferCommand, Result>
    {
        private readonly D2DContext _context;
        public DeclineCustomerOfferCommandHandler(D2DContext context)
        {
            _context= context;
        }
        public async Task<Result> Handle(DeclineCustomerOfferCommand request, CancellationToken cancellationToken)
        {
            var offer = await _context.CustomerCustomOffers.Where(o => o.ID == request.OfferId && o.CustomerOfferStatus == OfferStatus.OnHold).ExecuteDeleteAsync(cancellationToken);
            if (offer == 0)
                return Result.Failure(Messages.NotFound.WithTarget("Offer"));
            return Result.Success();
        }
    }
}
