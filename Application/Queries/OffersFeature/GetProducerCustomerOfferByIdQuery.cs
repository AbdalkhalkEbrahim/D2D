using Application.Response;
using Domain.DTOs.OfferDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.OffersFeature
{
    public class GetProducerCustomerOfferByIdQuery:IRequest<Result<ProducerOfferResponse>>
    {
        public Guid OfferId { get; set; }
    }
}
