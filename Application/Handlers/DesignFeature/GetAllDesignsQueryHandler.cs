using Application.Queries.DesignFeature;
using Application.Response;
using Domain.DTOs.DesignDtos;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.DesignFeature
{
    public class GetAllDesignsQueryHandler : IRequestHandler<GetAllDesignsQuery, Result<List<DesignResponse>>>
    {
        private readonly D2DContext _context;

        public GetAllDesignsQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<DesignResponse>>> Handle(GetAllDesignsQuery request, CancellationToken cancellationToken)
        {
           var query = _context.CustomerDesigns.Include(i=>i.DesignImages).AsNoTracking().Where(c=>c.CustomerId==request.CustomerId);
                
            if (await query.AnyAsync() == false)//
                return Result<List<DesignResponse>>.Failure(Messages.NotFound.WithTarget("User"));

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(d => d.Name == request.Name);

            if (request.Status.HasValue)
                query = query.Where(d => d.Status == request.Status.Value);
            if (request.StartDate.HasValue)
            {
                if (request.EndDate.HasValue)
                    query = query.Where(d => d.CreatedAt >= request.StartDate.Value && d.CreatedAt <= request.EndDate.Value);
                else
                    query = query.Where(d => d.CreatedAt == request.StartDate.Value);
            }
            //query = query.Skip((request.PageNum - 1) * request.PageSize).Take(request.PageSize);

            var result = new List<DesignResponse>();
            foreach (var item in query)
            {
                result.Add
                    (
                         new DesignResponse
                         {
                             CreatedAt = item.CreatedAt,
                             Id = item.ID,
                             Name = item.Name,
                             Images = item.DesignImages.Select(i => i.ImageUrl).ToList(),
                             PublishedAt = item.CreatedAt,
                             Status = item.Status.ToString(),
                         }
                    );
            }
            return result;
        }

    }
}
