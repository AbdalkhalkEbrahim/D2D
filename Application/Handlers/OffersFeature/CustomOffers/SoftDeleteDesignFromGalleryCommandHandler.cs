using Application.Commands.OffersFeature.CustomOffer;
using Application.Response;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class SoftDeleteDesignFromGalleryCommandHandler : IRequestHandler<SoftDeleteDesignFromGalleryCommand, Result>
    {
        private readonly D2DContext _context;
        public SoftDeleteDesignFromGalleryCommandHandler(D2DContext context)
        {
            _context=context;
        }
        public async Task<Result> Handle(SoftDeleteDesignFromGalleryCommand request, CancellationToken cancellationToken)
        {
            var deletedDesign= await _context.ProducerDesigns.Where(d=>d.ID==request.DesignId)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.IsDeleted,
                    u => true
                ), cancellationToken);

            if (deletedDesign == 0)
                return Result.Failure(Messages.NotFound.WithTarget("Design"));
            return Result.Success();
        }
    }
}
