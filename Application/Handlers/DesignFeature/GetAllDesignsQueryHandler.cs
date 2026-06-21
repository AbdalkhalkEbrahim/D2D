using Application.Queries.DesignFeature;
using Application.Response;
using Domain.DTOs.DesignDtos;
using Infrastructure.Data.Context;
using MediatR;
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
            return new List<DesignResponse>();
        }
    }
}
