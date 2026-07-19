using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class PushToProducerGalleryCommand:IRequest<Result<Guid>>
    {
        public string ProducerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Category {  get; set; }
        public string? Location { get; set; }
        public List<IFormFile> Images { get; set; } 
    }
}
