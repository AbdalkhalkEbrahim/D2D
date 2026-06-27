using Application.Response;
using Domain.DTOs.Review_RateDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Review_RateFeature
{
    public class AddRateCommand : IRequest<Result<RateResponse>>
    {
        public string ProducerId { get; set; }
        public double Rate { get; set; }
    }
}
