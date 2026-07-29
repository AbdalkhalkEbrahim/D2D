using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature.CustomOffer
{
    public class RemoveFromFavouritesCommand: IRequest<Result>
    {
        public string CustomerId { get; set; }
        public Guid DesignId { get; set; }
    }
}
