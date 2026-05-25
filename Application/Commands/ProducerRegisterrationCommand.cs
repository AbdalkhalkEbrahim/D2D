using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class ProducerRegisterrationCommand:IRequest<JwtToken>
    {
        public string ProducerId { get; set; }
        public IFormFile FrontImageID { get; set; }
        public IFormFile BackImageID { get; set; }
        public IFormFile PersonalImage { get; set; }
        public List<IFormFile> LicenseUrls { get; set; }
    }
}
