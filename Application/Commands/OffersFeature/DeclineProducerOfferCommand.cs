using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature
{
    public class DeclineProducerOfferCommand:IRequest<Result>
    {
        public Guid OfferId { get; set; }
        public Guid ProducerId { get; set; }
    }
}
