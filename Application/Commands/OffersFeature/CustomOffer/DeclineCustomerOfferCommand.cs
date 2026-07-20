using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature.CustomOffer
{
    public class DeclineCustomerOfferCommand : IRequest<Result>
    {
        public Guid OfferId { get; set; }
    }
}
