using Application.Commands.OffersFeature.CustomOffer;
using Application.Response;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class AddToFavouriteCommandHandler : IRequestHandler<AddToFavouriteCommand, Result>
    {
        private readonly D2DContext _context;

        public AddToFavouriteCommandHandler(D2DContext context)
        {
            _context= context;
        }
        public async Task<Result> Handle(AddToFavouriteCommand request, CancellationToken cancellationToken)
        {
            if (!_context.Users.Any(u => u.Id == request.CustomerId && !u.IsDeleted))
                return Result.Failure(Messages.NotFound.WithTarget("User"));
            
            if(!_context.ProducerDesigns.Any(d => d.ID == request.DesignId && !d.IsDeleted))
                return Result.Failure(Messages.NotFound.WithTarget("Design"));

            if (_context.Favourites.Any(f => f.UserId == request.CustomerId && f.ProducerDesignId == request.DesignId))
                return Result.Failure(new Error("Conflict", "Design is already in favourites."));

            var favourites = new Favourite
            {
                ProducerDesignId = request.DesignId,
                UserId = request.CustomerId
            };


            _context.Add(favourites);
            await _context.SaveChangesAsync();
            
           
            return Result.Success();
        }
    }
}
