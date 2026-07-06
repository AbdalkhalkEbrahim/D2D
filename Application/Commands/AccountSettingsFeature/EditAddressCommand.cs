using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.DTOs.PublishedDesignDtos;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class EditAddressCommand:IRequest<Result<AddressResponse>>
    {
        public int AddressId { get; set; }
        public required string CustomerId { get; set; }
        public bool Selected { get; set; }
        public JsonPatchDocument<AddressRequst> data { get; set; }

    }
}
