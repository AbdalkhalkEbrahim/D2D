using Application.Response;
using Domain.DTOs.DesignDtos;
using Google.Apis.Util;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DesignFeature
{
    public class SaveDesignCommand:IRequest<Result<List<Guid>>>
    {
        public string Id {  get; set; }
        public List<DesignsSaved> DesignsSaved { get; set; }
    }
}
