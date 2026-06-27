using Application.Commands.OffersFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature
{
    public class CustomerPublishOfferCommandHandler : IRequestHandler<CustomerPublishOfferCommand, Result<Guid>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CustomerPublishOfferCommandHandler(D2DContext context, IUploadService uploadService,IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _uploadService = uploadService;
            _hubContext = hubContext;
        }
        public async Task<Result<Guid>> Handle(CustomerPublishOfferCommand request, CancellationToken cancellationToken)
        {

            var designData = await _context.CustomerDesigns
            .Where(cd => cd.ID == request.DesignId)
            .Select(cd => new
            {
                cd.CustomerId,
                HasAlreadyPublished = _context.CustomerPublishedOffers.Any(cpo => cpo.CustomerDesignID == request.DesignId)
            })
            .FirstOrDefaultAsync(cancellationToken);
           // var design = await _context.CustomerDesigns.AsNoTracking().FirstOrDefaultAsync(cd => cd.ID == request.DesignId);
           // if (design == null)
           if(designData == null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("Design"));

           // var exsistingOffer = await _context.CustomerPublishedOffers.AnyAsync(cpo => cpo.CustomerDesignID == request.DesignId);
           // if (exsistingOffer)
           if(designData.HasAlreadyPublished)
                return Result<Guid>.Failure(Messages.Conflict.WithTarget("Offer"));

            var offer = new CustomerPublishedOffer
            {
                CustomerID = designData.CustomerId,//design.CustomerId,
                CustomerDesignID = request.DesignId,
                Category = request.Category,
                Description = request.Description,
                TargetAudience = request.TargetAudience,
                Gender = request.Gender,
                Colors = request.Colors,
                Material = request.Material,
                Amount = request.Amount,
                Duration = request.Duration,
                MaxPrice = request.MaxPrice,
                PrintingType = request.PrintingType,
                Sizes = request.Sizes
            };

            /*  design.UpdatedAt = DateTime.UtcNow;
              _context.Attach(design);
              _context.Entry(design).Property(d => d.UpdatedAt).IsModified = true;*/

            var designStub = new CustomerDesign { ID = request.DesignId, UpdatedAt = DateTime.UtcNow };
            _context.CustomerDesigns.Attach(designStub);
            _context.Entry(designStub).Property(d => d.UpdatedAt).IsModified = true;

            _context.Add(offer);

            if (request.SizesFile != null)
            {
                var file = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.SizesFile });
                BackgroundJob.Enqueue<IUploadService>(uploadService =>  uploadService.UploadAndSaveSingleFile(offer, "SizesFile", file[0], true));
            }
            //    await _hubContext.Clients.Group("ProducersGroup").SendAsync("onDesignPuplished", new { Message = "A new offer has been published." });
            BackgroundJob.Enqueue<INotificationService>(notificationService => notificationService.SendPuplishedDesignNotificationAsync(offer.ID));

            await _context.SaveChangesAsync();
            return offer.ID;

        }
    }
}
