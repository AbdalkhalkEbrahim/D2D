using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Entities.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.OffersFeature
{
    public class GetPublishedDesignByIdQuery:IRequest<Result<CustomerOfferResponse>>
    {
        public Guid OfferId { get; set; }
    }
}
