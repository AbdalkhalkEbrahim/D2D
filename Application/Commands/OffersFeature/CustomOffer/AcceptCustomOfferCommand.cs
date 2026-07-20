using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature.CustomOffer
{
    public class AcceptCustomOfferCommand : IRequest<Result<int>>
    {
        public Guid ProducerOfferId { get; set; }
        public decimal Amount { get; set; }

    }
}
