using Application.Commands.OffersFeature;
using Application.Response;
using Domain.Entities.Chats;
using Domain.Entities.Offers;
using Domain.Entities.Payment;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Handlers.OffersFeature
{
    public class AcceptOfferCommandHandler : IRequestHandler<AcceptOfferCommand, Result<int>>
    {
        private readonly D2DContext _context;
        private readonly IConfiguration _configuration;
        public AcceptOfferCommandHandler(D2DContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<Result<int>> Handle(AcceptOfferCommand request, CancellationToken cancellationToken)
        {
            var producerOffer = await _context.ProducerCustomerOffers
                .Include(po => po.CustomerPublishedOffer)
                .FirstOrDefaultAsync(po => po.ID == request.ProducerOfferId && po.OfferStatus == OfferStatus.OnHold, cancellationToken);

            if (producerOffer == null)
                return Result<int>.Failure(Messages.NotFound.WithTarget("Offer"));

            if (producerOffer.OfferStatus == OfferStatus.Accepted)
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
            var trans = new Transaction { Amount = request.Amount, CreatedAt = DateTime.UtcNow, Type = TransactionType.Deposit, UserID = producerOffer.ProducerID, Currency="eg" };
            _context.Add(trans);
            producerOffer.OfferStatus = OfferStatus.Accepted;

            await _context.CustomerPublishedOffers
                .Where(o => o.ID == producerOffer.CustomerPublishedOfferID)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u => u.IsActive, true)
                    .SetProperty(u => u.CustomerOfferStatus, OfferStatus.Accepted));


            await _context.Users
                .Where(u => u.UserType == UserType.Admin)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.Balance,
                    u => u.Balance + request.Amount*0.15m
                ), cancellationToken);


            trans = new Transaction { Amount = request.Amount*0.15m, CreatedAt = DateTime.UtcNow, Type = TransactionType.Deposit, UserID = _configuration["AdminId"], Currency="eg" };
            _context.Add(trans);


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

            var activeLog = new ActiveOfferLogs
            {
                Chat = chat,
                Step= ActiveOfferStatus.Negotiating.ToString(),
                PublishedOfferID = producerOffer.CustomerPublishedOffer.ID,
                CreatedAt = DateTime.UtcNow,
                IsPublishedOfferActive = true

            };


            await _context.ActiveOfferLogs.AddAsync(activeLog, cancellationToken);
            //_context.Attach(activation);
            producerOffer.OfferStatus = OfferStatus.Accepted;
            //_context.Entry(activation).Property(a=>a.IsActive).IsModified = true;
            await _context.SaveChangesAsync(cancellationToken);


            return chat.ID;



        }
    }
}
