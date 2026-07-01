using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature
{
    public class EditProducerCustomerOfferCommand:IRequest<Result<Guid>>
    {
        public Guid OfferId { get; set; }
        public string ProducerId { get; set; }
        public decimal Price { get; set; }
    }
}
