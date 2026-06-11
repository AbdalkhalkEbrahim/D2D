using Application.Response;
using Domain.DTOs.RegisterationDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.RegisterationFeature
{
    public class DesignerRegesterationCommand : IRequest<Result<DesignerRegisterationResponse>>
    {
        [Required]
        public required string DesignerId { get; set; }
        [Required]
        public required IFormFile FrontImageID { get; set; }
        [Required]
        public required IFormFile BackImageID { get; set; }
        [Required]
        public required IFormFile PersonalImage { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "At least three steps are required.")]

        public required List<IFormFile> StepUrls { get; set; }
    }
}
