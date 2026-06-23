using Application.Commands.OffersFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Offers;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature
{
    public class PublishOfferCommandHandler : IRequestHandler<PublishOfferCommand, Result<Guid>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;

        public PublishOfferCommandHandler(D2DContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }
        public async Task<Result<Guid>> Handle(PublishOfferCommand request, CancellationToken cancellationToken)
        {
            
            var design = await _context.CustomerDesigns.AsNoTracking().FirstOrDefaultAsync(cd => cd.ID == request.DesignId);
            if (design == null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("Design"));

            var exsistingOffer = await _context.CustomerPublishedOffers.AnyAsync(cpo => cpo.CustomerDesignID == request.DesignId);
            if (exsistingOffer)
                return Result<Guid>.Failure(Messages.Conflict.WithTarget("Offer"));

            var offer = new CustomerPublishedOffer
            {
                CustomerID = design.CustomerId,
                CustomerDesignID = request.DesignId,
                Category = request.Category,
                Description = request.Description,
                TargetAudience = request.TargetAudience,
                Gender = request.Gender,
                Season = request.Season,
                Style = request.Style,
                Colors = request.Colors,
                Material = request.Material,
                Amount = request.Amount,
                Duration = request.Duration,
                MaxPrice = request.MaxPrice,
                PrintingType = request.PrintingType,
                Sizes = request.Sizes,
                CreatedAt = DateTime.UtcNow
            };

            design.UpdatedAt = DateTime.UtcNow;
            _context.Attach(design);
            _context.Entry(design).Property(d => d.UpdatedAt).IsModified = true;

            _context.Add(offer);

            if (request.SizesFile != null)
            {
                var file = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.SizesFile });
                BackgroundJob.Enqueue<IUploadService>(uploadService => uploadService.UploadAndSaveSingleFile(offer, "SizesFile", file[0], true));
            }

            await _context.SaveChangesAsync();
            return offer.ID;

        }
    }
}
