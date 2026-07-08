using Application.Commands.ReviewFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Entities.Producers;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ReviewFeature
{
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, Result<ReviewResponse>>
    {
        private readonly D2DContext _context;
        public AddReviewCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ReviewResponse>> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var producer = await _context.Producers.Where(u => u.Id == request.ProducerId).Select(p=>new
            {
                CustomerAnonName = p.Reviews.FirstOrDefault(r=>r.CustomerID == request.CustomerId).Customer.AnonName,
                ProducerAnonName = p.AnonName,
                p.RateCount, p.Rate

            }).FirstOrDefaultAsync();

            if (producer == null)
                return Result<ReviewResponse>.Failure(Messages.NotFound.WithTarget("User"));

            var review = new Review
            {
                Content = request.Content,
                Rate = request.Rate,
                ProducerID = request.ProducerId,
                CustomerID = request.CustomerId,
                CreatedAt = DateTime.UtcNow
            };

            var updatedProducer = new Producer { Id = request.ProducerId, RateCount = producer.RateCount + 1, Rate = producer.Rate + request.Rate };
            _context.Attach(updatedProducer);
            _context.Entry(updatedProducer).Property(p => p.RateCount).IsModified = true;

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return new ReviewResponse
            {
                Content = request.Content,
                Rate = request.Rate,
                ProducerRate = updatedProducer.Rate/updatedProducer.RateCount,
                ProducerAnonName = producer.ProducerAnonName,
                CustomerAnonName = producer.CustomerAnonName,
                ProducerId = request.ProducerId,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
