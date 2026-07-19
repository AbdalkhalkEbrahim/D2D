using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.DTOs.PublishedDesignDtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        //public IFormFile? ProfileImageUrl { get; set; }
        public JsonPatchDocument<ProfileRequest> data { get; set; }

    }
}
