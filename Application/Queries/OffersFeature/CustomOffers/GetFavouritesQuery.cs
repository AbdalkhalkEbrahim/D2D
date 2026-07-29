using Application.Response;
using Domain.Entities.Offers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.OffersFeature.CustomOffers
{
    public class GetFavouritesQuery : IRequest<Result<List<GetFavouritesResponse>>>
    {
        public string CustomerId { get; set; }
    }
}
