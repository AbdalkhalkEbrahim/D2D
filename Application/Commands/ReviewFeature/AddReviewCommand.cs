using Application.Response;
using Domain.DTOs.OfferDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ReviewFeature
{
    public class AddReviewCommand:IRequest<Result<ReviewResponse>>
    {
        public string ProducerId { get; set; }
        public int Rate { get; set; }
        public string Content { get; set; }
        public string CustomerId { get; set; }
    }
}
