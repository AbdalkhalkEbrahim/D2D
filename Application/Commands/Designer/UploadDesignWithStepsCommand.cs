using Application.Response;
using Domain.DTOs.Designer;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Designer
{
    public class UploadDesignWithStepsCommand:IRequest<Result<UploadDesignResponse>>
    {
        public string DesignerId { get; set; }
        public IFormFile Design {  get; set; }
        public List<IFormFile> Steps { get; set; }
    }
}
