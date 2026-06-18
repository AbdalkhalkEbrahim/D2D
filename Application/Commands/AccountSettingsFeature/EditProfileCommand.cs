using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class EditProfileCommand : IRequest<Result<ProfileResponse>>
    { 
        public required string UserId { get; set; }
        [RegularExpression("^[a-zA-O-Z-a-z]{1,30}$", ErrorMessage = "Invalid name format")]
        public string? FirstName { get; set; }
        [RegularExpression("^[a-zA-O-Z-a-z]{1,30}$", ErrorMessage = "Invalid name format")]
        public string? LastName { get; set; }
        public IFormFile? ProfileImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
