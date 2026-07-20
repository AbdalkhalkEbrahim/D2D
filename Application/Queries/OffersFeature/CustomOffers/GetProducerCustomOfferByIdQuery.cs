using Application.Response;
using Domain.DTOs.OfferDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.OffersFeature.CustomOffers
{
    public class GetProducerCustomOfferByIdQuery: IRequest<Result<ProducerOfferResponse>>
    {
        public Guid ProducerOfferId { get; set; }
    }
}
