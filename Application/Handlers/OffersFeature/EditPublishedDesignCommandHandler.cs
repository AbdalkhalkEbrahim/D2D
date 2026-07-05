using Application.Commands.OffersFeature;
using Application.Response;
using Azure;
using Domain.Entities.Offers;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using jsonPatch= Microsoft.AspNetCore.JsonPatch.Operations ;
using Microsoft.EntityFrameworkCore;
using Domain.Enums.Status;

namespace Application.Handlers.OffersFeature
{
    public class EditPublishedDesignCommandHandler : IRequestHandler<EditPublishedDesignCommand, Result<Guid>>
    {
        private readonly D2DContext _context;
        public EditPublishedDesignCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<Guid>> Handle(EditPublishedDesignCommand request, CancellationToken cancellationToken)
        {
            var design = await _context.CustomerPublishedOffers.Include(cpo=>cpo.ActiveOfferLogs).FirstOrDefaultAsync(d => d.ID == request.CustomerPublishedOfferId && (!d.IsActive || (d.IsActive && d.ActiveOfferLogs.Count() == 1 )));
            if (design == null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("Design"));

            var entityPatch = new JsonPatchDocument<CustomerPublishedOffer>();
            request.data.Operations.ForEach(op => entityPatch.Operations.Add(new jsonPatch.Operation<CustomerPublishedOffer>(op.op, op.path, op.from, op.value)));
           // Console.WriteLine(request.data.Operations.Count);
            entityPatch.ApplyTo(design);
           // Console.WriteLine(design.Category);
           // _context.CustomerPublishedOffers.Update(design);
            await _context.SaveChangesAsync();

            return design.ID;
        }
    }
}
