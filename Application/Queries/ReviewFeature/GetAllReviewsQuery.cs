using Application.Response;
using Domain.DTOs;
using Domain.DTOs.Review_RateDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ReviewFeature
{
    public class GetAllReviewsQuery:IRequest<Result<ReviewAndRateResponse>>
    {
        public string ProducerId { get; set; }
        public int PageSize { get; set; } = 6;
        public int PageNum { get; set; } = 1;
    }
}
