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
    public class GetAllPublishedesignsQuery:IRequest<Result<List<CustomerOfferResponse>>>
    {
        public int Gender { get; set; }
        public string? Category { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public int MaxPrice { get; set; }
        public int CreationOrder { get; set; }
        public int? PageSize { get; set; } = 10;
        public int? PageNum { get; set; } = 1;
    }
}
