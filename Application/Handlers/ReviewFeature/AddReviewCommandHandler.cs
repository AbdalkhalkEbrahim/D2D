using Application.Commands.ReviewFeature;
using Application.Response;
using Domain.DTOs;
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
            var producer = await _context.Producers.FirstOrDefaultAsync(u => u.Id == request.ProducerId);
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
            producer.Reviews.Add(review);
            _context.Update(producer);
            await _context.SaveChangesAsync();
            return new ReviewResponse
            {
                Content = request.Content,
                Rate = request.Rate,
                ProducerId = request.ProducerId,
                CustomerId = request.CustomerId,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
