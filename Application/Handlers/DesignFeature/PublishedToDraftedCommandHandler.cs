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
           var design =await _context.CustomerDesigns.Include(d=>d.CustomerPublishedOffer).FirstOrDefaultAsync(d => d.ID == request.DesignId && d.Status==DesignStatus.Published);
            if (design == null)
                return Result<Guid>.Failure(Messages.BadRequest.WithTarget("DraftedDesign"));

            design.Status = DesignStatus.Drafted;
            _context.Remove(design.CustomerPublishedOffer); 
            _context.SaveChanges();

            return request.DesignId;
        }
    }
}
