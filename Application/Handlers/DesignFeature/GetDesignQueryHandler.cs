using Application.Queries.DesignFeature;
using Application.Response;
using Domain.DTOs.DesignDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.DesignFeature
{
    public class GetDesignQueryHandler : IRequestHandler<GetDesignQuery, Result<DesignResponse>>
    {
        private readonly D2DContext _context;

        public GetDesignQueryHandler(D2DContext context)
        {
            _context = context;   
        }
        public async Task<Result<DesignResponse>> Handle(GetDesignQuery request, CancellationToken cancellationToken)
        {
            var design = await _context.CustomerDesigns.AsNoTracking().Include(d=>d.DesignImages).FirstOrDefaultAsync(d => d.ID == request.Id);
            if (design == null)
                return Result<DesignResponse>.Failure(Messages.NotFound.WithTarget("Design"));

            return new DesignResponse
            {
                Id = design.ID,
                Name = design.Name,
                Status = design.Status.ToString(),
                Images = new List<string> { design.DesignImages.Last().ImageUrl },
                CreatedAt = design.CreatedAt,
                PublishedAt = design.UpdatedAt
            };
        }
    }
}
