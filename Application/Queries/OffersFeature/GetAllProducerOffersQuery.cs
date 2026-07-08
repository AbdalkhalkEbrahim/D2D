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
    public class GetAllProducerOffersQuery:IRequest<Result<List<ProducerOfferResponse>>>
    {

        public string ProducerId { get; set; }
        public bool Newest { get; set; } = true;
        public bool IsAccepted { get; set; }
        public bool IsPending { get; set; }
        public bool IsDecilned { get; set; }
        public int? PageNum { get; set; } = 1;
        public int? PageSize { get; set; } = 4;
    }
}
