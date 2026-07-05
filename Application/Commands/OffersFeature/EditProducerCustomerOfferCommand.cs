using Application.Response;
using Domain.DTOs.PublishedDesignDtos;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
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
        public JsonPatchDocument<ProducerOfferRequest> data { get; set; }
    }
}
