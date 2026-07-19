using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.DTOs.PublishedDesignDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.OffersFeature.CustomOffer
{
    public class EditProducerPublishedDesignFromGalleryCommand:IRequest<Result>
    {
        public string ProducerId { get; set; }
        public Guid DesignId {  get; set; }
        public string? Name { get; set; }
       // public List<IFormFile>? Designs { get; set; }
        public JsonPatchDocument<ProducerPublishedDesignFromGallery> data { get; set; }
    }
}
