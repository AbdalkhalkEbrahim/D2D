using Application.Commands.OffersFeature.CustomOffer;
using Application.Response;
using Domain.Entities.Chats;
using Domain.Entities.Offers;
using Domain.Entities.Payment;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class AcceptCustomOfferCommandHandler : IRequestHandler<AcceptCustomOfferCommand, Result<int>>
    {
        private readonly D2DContext _context;
        private readonly IConfiguration _configuration;
        public AcceptCustomOfferCommandHandler(D2DContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<Result<int>> Handle(AcceptCustomOfferCommand request, CancellationToken cancellationToken)
        {
            var producerOffer = await _context.ProducerCustomerOffers
               .Include(po => po.CustomerCustomOffer)
               .FirstOrDefaultAsync(po => po.ID == request.ProducerOfferId && po.OfferStatus == OfferStatus.OnHold, cancellationToken);

            if (producerOffer == null)
                return Result<int>.Failure(Messages.NotFound.WithTarget("Offer"));

            if (producerOffer.OfferStatus== OfferStatus.Accepted)
                return Result<int>.Failure(Messages.Conflict.WithTarget("Active"));

            if (request.Amount != producerOffer.Diposit)
            {
                return Result<int>.Failure(Messages.BadRequest.WithTarget("Balance"));
            }


            var customer = await _context.Customers
                .FirstOrDefaultAsync(u => u.Id == producerOffer.CustomerCustomOffer.CustomerID, cancellationToken);

            if (customer.Balance < request.Amount)
                return Result<int>.Failure(Messages.BadRequest.WithTarget("PriceMismatch"));
            customer.Balance -= request.Amount;

            producerOffer.OfferStatus = OfferStatus.Accepted;
            var trans = new Transaction { Amount = request.Amount, CreatedAt = DateTime.UtcNow, Type = TransactionType.Deposit, UserID = producerOffer.ProducerID, Currency = "eg" };
            _context.Add(trans);

            await _context.CustomerCustomOffers
            .Where(o => o.ID == producerOffer.CustomOfferId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.IsActive, true)
                .SetProperty(u => u.CustomerOfferStatus, OfferStatus.Accepted));


            await _context.Users
                .Where(u => u.UserType == UserType.Admin)
                .ExecuteUpdateAsync(s => s.SetProperty(
                    u => u.Balance,
                u => u.Balance + request.Amount * 0.15m
                ), cancellationToken);


            trans = new Transaction { Amount = request.Amount * 0.15m, CreatedAt = DateTime.UtcNow, Type = TransactionType.Deposit, UserID = _configuration["AdminId"], Currency = "eg" };
            _context.Add(trans);

            var chat = new Chat
            {
                CustomerID = producerOffer.CustomerCustomOffer.CustomerID,
                ProducerID = producerOffer.ProducerID,
                Name = producerOffer.CustomerCustomOffer.Name
            };
            await _context.Chats.AddAsync(chat, cancellationToken);

            var activeLog = new ActiveOfferLogs
            {
                Chat = chat,
                Step = ActiveOfferStatus.Negotiating.ToString(),
                CustomOfferID = producerOffer.CustomerCustomOffer.ID,
                CreatedAt = DateTime.UtcNow,
                IsCustomOfferActive = true

            };
            await _context.ActiveOfferLogs.AddAsync(activeLog, cancellationToken);
            producerOffer.OfferStatus= OfferStatus.Accepted;
            //_context.Attach(activation);

            //_context.Entry(activation).Property(a=>a.IsActive).IsModified = true;
            await _context.SaveChangesAsync(cancellationToken);


            return chat.ID;
        }
    }
}
