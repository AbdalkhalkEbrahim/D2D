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
    public class PushToProducerGalleryCommand:IRequest<Result>
    {
        public string ProducerId { get; set; }
        public string Description { get; set; }
        public List<IFormFile> Images { get; set; } 
    }
}
