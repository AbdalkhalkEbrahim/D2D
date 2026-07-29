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
    public class GetCustomRequestByIdQuery : IRequest<Result<CustomerOfferResponse>>
    {
        public Guid OfferId { get; set; }
    }
}
