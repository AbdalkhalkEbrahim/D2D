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
        public List<IFormFile> IdentityFiles { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one license file is required.")]
        public List<IFormFile> LicenseUrls { get; set; } = new();
    }
}
