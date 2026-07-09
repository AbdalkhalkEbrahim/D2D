using Application.Commands.DesignFeature;
using Application.Response;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.DesignFeature
{
    public class PublishedToDraftedCommandHandler : IRequestHandler<PublishedToDraftedCommand, Result<Guid>>
    {
        private readonly D2DContext _context;
        public PublishedToDraftedCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> Handle(PublishedToDraftedCommand request, CancellationToken cancellationToken)
        {
           var design =await _context.CustomerDesigns.Include(d=>d.CustomerPublishedOffer).ThenInclude(c=>c.ProducerCustomerOffers).FirstOrDefaultAsync(d => d.ID == request.DesignId && d.Status==DesignStatus.Published);
            if (design == null)
                return Result<Guid>.Failure(Messages.BadRequest.WithTarget("DraftedDesign"));

            design.Status = DesignStatus.Drafted;
            if(!_context.ActiveOfferLogs.Any(al=>al.CustomOfferID == design.CustomerPublishedOfferID))
                return Result<Guid>.Failure(Messages.Conflict.WithTarget("ActiveDesign"));

            foreach (var d in design.CustomerPublishedOffer.ProducerCustomerOffers)
            {
                _context.Attach(d);
                d.OfferStatus = OfferStatus.Declined;
                _context.Entry(d).Property(pco=>pco.OfferStatus).IsModified = true;
            }
            _context.SaveChanges();

            return request.DesignId;
        }
    }
}
