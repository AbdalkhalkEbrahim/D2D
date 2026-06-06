using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class ProducerRegisterrationCommand:IRequest<object>
    {

        [Required]
        public string ProducerId { get; set; } = string.Empty;

        [Required]
        public IFormFile FrontImageID { get; set; }

        [Required]
        public IFormFile BackImageID { get; set; }

        [Required]
        public IFormFile PersonalImage { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one license file is required.")]
        public List<IFormFile> LicenseUrls { get; set; } = new();
    }
}
