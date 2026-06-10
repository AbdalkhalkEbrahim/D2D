using Application.Response;
using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class CustomerRegisterationCommand : IRequest<Result<CustomerRegisteratonResponse>>
    {
        [Required]
        public required string CustomerId { get; set; }
        [Required]

        public required IFormFile FrontImageID { get; set; }
        [Required]
        public required IFormFile BackImageID { get; set; }
        [Required]
        public required IFormFile PersonalImage { get; set; }
        [Required]
        [MaxLength(2)]
        public required string AppartmentNo { get; set; }

        [Required]
        [Range(1, 70, ErrorMessage = "Building number must be between 1 and 70.")]
        public int BuildingNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public required string Street { get; set; }

        [Required]
        [MaxLength(30)]
        public required string District { get; set; }

        [Required]
        [MaxLength(30)]
        public required string City { get; set; }

        [Required]
        [MaxLength(30)]
        public required string Goverate { get; set; }
    }

}