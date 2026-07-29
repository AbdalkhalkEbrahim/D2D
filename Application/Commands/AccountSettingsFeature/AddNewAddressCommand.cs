using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.AccountSettingsFeature
{
    public class AddNewAddressCommand : IRequest<Result<int>>
    {
        public required string CustomerId { get; set; }
        [Required]
        [MaxLength(2)]
        [Range(1, 70, ErrorMessage = "Building number must be between 1 and 70.")]

        public required string AppartmentNo { get; set; }

        [Required]
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
        public bool Selected { get; set; }=false;
    }
}
