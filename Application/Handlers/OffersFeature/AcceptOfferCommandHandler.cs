using Application.Commands.OffersFeature;
using Application.Response;
using Domain.Entities.Chats;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AL = Domain.Entities.Offers;

namespace Application.Handlers.OffersFeature
{
    public class AcceptOfferCommandHandler : IRequestHandler<AcceptOfferCommand, Result<int>>
    {
        private readonly D2DContext _context;

        public AcceptOfferCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<int>> Handle(AcceptOfferCommand request, CancellationToken cancellationToken)
        {
            var producerOffer = await _context.ProducerCustomerOffers
                .Include(po => po.CustomerPublishedOffer)
                .FirstOrDefaultAsync(po => po.ID == request.ProducerOfferId && po.OfferStatus == OfferStatus.OnHold, cancellationToken);

            if (producerOffer == null)
                return Result<int>.Failure(Messages.NotFound.WithTarget("Offer"));

            if (producerOffer.CustomerPublishedOffer.IsActive)
                return Result<int>.Failure(Messages.Conflict.WithTarget("Active"));

            if (request.Amount != producerOffer.Diposit)
            {
                return Result<int>.Failure(Messages.BadRequest.WithTarget("Balance"));
            }


            var customer = await _context.Customers
                .FirstOrDefaultAsync(u => u.Id == producerOffer.CustomerPublishedOffer.CustomerID, cancellationToken);

            if(customer.Balance < request.Amount)
                return Result<int>.Failure(Messages.BadRequest.WithTarget("PriceMismatch"));

            customer.Balance -= request.Amount;
            
            producerOffer.OfferStatus = OfferStatus.Accepted;

            await _context.CustomerPublishedOffers.Where(o => o.ID == producerOffer.CustomerPublishedOfferID)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.IsActive,
                    u => true));

            await _context.Users
                .Where(u => u.UserType == UserType.Admin)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.Balance,
                    u => u.Balance + request.Amount*0.15m
                ), cancellationToken);


            await _context.ProducerCustomerOffers.Where(po => po.ID != request.ProducerOfferId && po.CustomerPublishedOfferID == producerOffer.CustomerPublishedOfferID).ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.OfferStatus,
                    u => OfferStatus.Declined));

            var chat = new Chat
            {
                CustomerID = producerOffer.CustomerPublishedOffer.CustomerID,
                ProducerID = producerOffer.ProducerID,
                Name = producerOffer.CustomerPublishedOffer.Name
            };
            await _context.Chats.AddAsync(chat, cancellationToken);

            var activeLog = new AL.ActiveOfferLogs
            {
                Chat = chat,
                Step= ActiveOfferStatus.Negotiating.ToString(),
                PublishedOfferID = producerOffer.CustomerPublishedOffer.ID,
                CreatedAt = DateTime.UtcNow,
                IsPublishedOfferActive = true

            };
            await _context.ActiveOfferLogs.AddAsync(activeLog, cancellationToken);
            //_context.Attach(activation);

            //_context.Entry(activation).Property(a=>a.IsActive).IsModified = true;
            await _context.SaveChangesAsync(cancellationToken);


            return chat.ID;



        }
    }
}
