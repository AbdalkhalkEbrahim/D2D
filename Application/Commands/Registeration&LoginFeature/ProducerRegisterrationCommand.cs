using Application.Response;
using Domain.DTOs.RegisterationDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.RegisterationFeature
{
    public class ProducerRegisterrationCommand : IRequest<Result<ProducerRegisterationResponse>>
    {

        [Required]
        public required string ProducerId { get; set; }

        [Required]
        public required IFormFile FrontImageID { get; set; }

        [Required]
        public required IFormFile BackImageID { get; set; }

        [Required]
        public required IFormFile PersonalImage { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one license file is required.")]
        public List<IFormFile> LicenseUrls { get; set; } = new();
    }
}
