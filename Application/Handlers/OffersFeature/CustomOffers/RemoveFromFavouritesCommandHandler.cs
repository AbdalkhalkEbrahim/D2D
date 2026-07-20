using Application.Commands.OffersFeature.CustomOffer;
using Application.Response;
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
    public class RemoveFromFavouritesCommandHandler : IRequestHandler<RemoveFromFavouritesCommand, Result>
    {
        private readonly D2DContext _context;

        public RemoveFromFavouritesCommandHandler(D2DContext context)
        {
            _context= context;
        }
        public async Task<Result> Handle(RemoveFromFavouritesCommand request, CancellationToken cancellationToken)
        {
            var removedDesign = await _context.Favourites.Where(f=>f.UserId==request.CustomerId && !f.User.IsDeleted && f.ProducerDesignId==request.DesignId && !f.ProducerDesign.IsDeleted).
                ExecuteDeleteAsync(cancellationToken);

            if (removedDesign == 0)
                return Result.Failure(Messages.NotFound.WithTarget("Design"));

            return Result.Success();
        }
    }
}
