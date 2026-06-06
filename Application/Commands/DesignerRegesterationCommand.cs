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
    public class DesignerRegesterationCommand:IRequest<object>
    {
        [Required]
        public string DesignerId { get; set; }
        [Required]
        public IFormFile FrontImageID { get; set; }
        [Required]
        public IFormFile BackImageID { get; set; }
        [Required]
        public IFormFile PersonalImage { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "At least three steps are required.")]

        public List<IFormFile> StepUrls { get; set; }
    }
}
