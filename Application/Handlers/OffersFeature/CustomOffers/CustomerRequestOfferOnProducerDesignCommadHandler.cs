using Application.Commands.OffersFeature.CustomOffer;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Offers;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class CustomerRequestOfferOnProducerDesignCommadHandler : IRequestHandler<CustomerRequestOfferOnProducerDesignCommad, Result<Guid>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CustomerRequestOfferOnProducerDesignCommadHandler(D2DContext context,IUploadService uploadService,IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _uploadService = uploadService;
            _hubContext = hubContext;
        }
        public async Task<Result<Guid>> Handle(CustomerRequestOfferOnProducerDesignCommad request, CancellationToken cancellationToken)
        {
            var RequestedDesign =await _context.ProducerDesigns.Where(pd => pd.ID == request.DesignId && !pd.IsDeleted).
                Select(d => new {
                    d.ProducerID,
                    d.Name,
                    IsAlreadyRequested= _context.CustomerCustomOffers.Any(cco=>cco.CustomerID==request.CustomerId && cco.ProducerDesignID==request.DesignId && cco.CustomerOfferStatus!=OfferStatus.Completed)
                }).FirstOrDefaultAsync();

            if (RequestedDesign==null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("Design"));

            if (RequestedDesign.IsAlreadyRequested)
                return Result<Guid>.Failure(Messages.Conflict.WithTarget("Offer"));

            var offer = new CustomerCustomOffer
            {
                CustomerID = request.CustomerId,
                Name = request.Name,
                ProducerDesignID = request.DesignId,
                Category = request.Category,
                Description = request.Description,
                TargetAudience = request.TargetAudience,
                Gender = request.Gender,
                Colors = request.Colors,
                Material = request.Material,
                Amount = request.Amount,
                Duration = request.Duration,
                MaxPrice = request.TargetPrice,
                PrintingType = request.PrintingType,
                Sizes = request.Sizes,
            };

            _context.Add(offer);

           
            if (request.SizesFile != null)
            {
                var file = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.SizesFile });
                BackgroundJob.Enqueue<IUploadService>(uploadService => uploadService.UploadAndSaveSingleFile(offer, "SizesFile", file[0], true));
            }
             await _hubContext.Clients.Client(RequestedDesign.ProducerID).SendAsync("onDesignPuplished", new { Message = $"you recieve a new offer on {RequestedDesign.Name} from your gallery" });

            var notification = new Notification
            {
                Content = $"you recieved a new offer on {RequestedDesign.Name} from your gallery ",
                NotificationsType= NotificationsType.RecieveOffer,
                UserID= RequestedDesign.ProducerID,
                Title="New Offer",
                RefrenceUrl = ""
            };
            _context.Add(notification);
            await _context.SaveChangesAsync();

            return offer.ID;

        }
    }
}
