using Application.Queries.OffersFeature.CustomOffers;
using Application.Response;
using Domain.Entities.Offers;
using Infrastructure.Data.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class GetFavouritesQueryHandler : IRequestHandler<GetFavouritesQuery, Result<List<GetFavouritesResponse>>>
    {
        private readonly D2DContext _context;

        public GetFavouritesQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<GetFavouritesResponse>>> Handle(GetFavouritesQuery request, CancellationToken cancellationToken)
        {
            var favourites= _context.Favourites.Where(f=>f.UserId==request.CustomerId && !f.ProducerDesign.IsDeleted).Select(f=>new GetFavouritesResponse
            {
                ProducerId = f.ProducerDesign.ProducerID,
                ProducerAnnon = f.ProducerDesign.Producer.AnonName,
                DesignId=f.ProducerDesign.ID,
                DesignImages= f.ProducerDesign.DesignImages.Select(i=>i.ImageUrl).ToList(),
                Name=f.ProducerDesign.Name,
                Category=f.ProducerDesign.Category,
                Location=f.ProducerDesign.Location,
                Notes=f.ProducerDesign.Notes,
                CreatedAt=f.ProducerDesign.CreatedAt,
                UpdatedAt=f.ProducerDesign.UpdatedAt,
                
            }).ToList();

            if (!favourites.Any())
                return Result<List<GetFavouritesResponse>>.Failure(new Error("NotFound", "you don't have any favourites yet"));
            return favourites;
        }
    }
}
