using Application.Response;
using Domain.DTOs.OfferDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature.CustomOffer
{
    public class ProducerCustomOfferCommand:IRequest<Result<ProducerOfferResponse>>
    {
        public string CustomerId { get; set; }
        public string ProducerId { get; set; }
        public Guid CustomerCustomOfferId { get; set; }
        public decimal Price { get; set; }
        public decimal Diposit { get; set; }
        public int DeliveryTime { get; set; }
        public Dictionary<string, (int, int)> Steps { get; set; }
    }
}
