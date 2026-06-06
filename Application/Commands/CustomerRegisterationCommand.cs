using Domain.DTOs;
using Domain.Entities.Customers;
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
    public class CustomerRegisterationCommand : IRequest<object>
    {
        [Required]
        public string CustomerId { get; set; }
        [Required]

        public IFormFile FrontImageID { get; set; }
        [Required]
        public IFormFile BackImageID { get; set; }
        [Required]
        public IFormFile PersonalImage { get; set; }
        [Required]
        [MaxLength(2)]
        public string AppartmentNo { get; set; }

        [Required]
        [Range(1, 70, ErrorMessage = "Building number must be between 1 and 70.")]
        public int BuildingNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public string Street { get; set; }

        [Required]
        [MaxLength(30)]
        public string District { get; set; }

        [Required]
        [MaxLength(30)]
        public string City { get; set; }

        [Required]
        [MaxLength(30)]
        public string Goverate { get; set; }
    }

}