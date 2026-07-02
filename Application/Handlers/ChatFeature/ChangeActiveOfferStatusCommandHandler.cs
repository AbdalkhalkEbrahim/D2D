using Application.Commands.ChatFeature;
using Application.Response;
using Domain.Entities.Offers;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Handlers.ChatFeature
{
    public class ChangeActiveOfferStatusCommandHandler : IRequestHandler<ChangeActiveOfferStatusCommand, Result<string>>
    {
        private readonly D2DContext _context;

        public ChangeActiveOfferStatusCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<string>> Handle(ChangeActiveOfferStatusCommand request, CancellationToken cancellationToken)
        {
            var activeOffer = await _context.ActiveOfferLogs.Where(ao => ao.ChatID == request.ChatID).Select(ao=> new { ao.ID, ao.Status }).FirstOrDefaultAsync();
            if(activeOffer == null) 
                return Result<string>.Failure(Messages.NotFound.WithTarget("Offer"));

            var active = new ActiveOfferLogs { ID = activeOffer.ID };
            _context.Attach(active);
            _context.Entry(active).Property(a => a.Status).CurrentValue = request.Status;
            _context.Entry(active).Property(a => a.UpdatedAt).CurrentValue = DateTime.UtcNow;
            _context.Entry(active).Property(a => a.UpdatedAt).IsModified = true;
            _context.Entry(active).Property(a => a.Status).IsModified = true;
            await _context.SaveChangesAsync();
            return active.Status.ToString();
        }
    }
}
