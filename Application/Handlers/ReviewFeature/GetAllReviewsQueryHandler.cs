using Application.Queries.ReviewFeature;
using Application.Response;
using Domain.DTOs;
using Domain.DTOs.Review_RateDtos;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Handlers.ReviewFeature
{
    public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, Result<ReviewAndRateResponse>>
    {
        private readonly D2DContext _context;

        public GetAllReviewsQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ReviewAndRateResponse>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
        {
            var producerReviews = await _context.Producers.Select(p => new
            {
                Reviews = p.Reviews.Select(r => new { r.ID, r.Content, r.Rate, r.CreatedAt, r.UpdatedAt, CName = r.Customer.AnonName, PName = r.Producer.AnonName }),
                p.Id,
                p.AnonName,
                SumRate = p.Rate,
                CountRate = p.RateCount
            }).FirstOrDefaultAsync(p => p.Id == request.ProducerId);

            if (request.PageSize < 6)
                request.PageSize = 6;

            if (request.PageNum < 1)
                request.PageNum = 1;

            //offer = offer.Skip((request.PageNum - 1) * request.PageSize).Take(request.PageSize);

            var response = new ReviewAndRateResponse { ProducerRate = producerReviews.SumRate/producerReviews.CountRate, TotalRates = producerReviews.CountRate };
            response.Reviews = new List<ReviewResponse>();
            foreach(var r in producerReviews.Reviews)
            {
                response.Reviews.Add(new ReviewResponse
                {
                    ID = r.ID,
                    Content = r.Content,
                    Rate = r.Rate,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = (DateTime)r.UpdatedAt,
                    CustomerAnonName = r.CName,
                    ProducerAnonName = r.PName,
                    ProducerId = request.ProducerId
                });
            }
            return response;
        }
    }
}
