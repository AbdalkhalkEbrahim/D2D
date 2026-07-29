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
    public class GetOffersOnPublishedDesignQuery:IRequest<Result<List<ProducerOfferResponse>>>
    {
        public Guid PublishedDesignId { get; set; }
        public bool LowestPrice { get; set; }
        public bool NewestOffer { get; set; }
        public bool BestMatch { get; set; }
        public bool HighestRate { get; set; }
        public int? PageSize { get; set; } = 5;
        public int? PageNum { get; set; } = 1;
    }
}
