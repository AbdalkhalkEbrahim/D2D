using Application.Response;
using Domain.DTOs.OfferDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature
{
    public class ProducerCustomerOfferCommand:IRequest<Result<ProducerOfferResponse>>
    {
        public string ProducerId { get; set; }
        public Guid CustomerPublishedOfferId { get; set; }
        public decimal Price { get; set; }

    }
}
