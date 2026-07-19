using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature
{
    public class DeleteProducerOfferCommand:IRequest<Result>
    {
        public Guid OfferId { get; set; }
    }
}
