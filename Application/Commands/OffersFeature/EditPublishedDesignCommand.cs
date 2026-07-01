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
        public Guid DesignId { get; set; }
        public JsonPatchDocument<PublishedDesign> data { get; set; }
    }
}
