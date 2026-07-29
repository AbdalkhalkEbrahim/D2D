using Application.Response;
using Azure;
using Domain.DTOs.PublishedDesignDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;

namespace Application.Commands.OffersFeature
{
    public class EditPublishedDesignCommand:IRequest<Result<Guid>>
    {
        public Guid CustomerPublishedOfferId { get; set; }
        public JsonPatchDocument<PublishedDesignRequest> data { get; set; }
    }
}
