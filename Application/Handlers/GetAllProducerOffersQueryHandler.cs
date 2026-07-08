using Application.Queries;
using Application.Response;
using Domain.DTOs;
using Domain.DTOs.OfferDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetAllProducerOffersQueryHandler : IRequestHandler<GetAllProducerOffersQuery, Result<CollaborationsWithCounts>>
    {
        private readonly D2DContext _context;

        public GetAllProducerOffersQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<CollaborationsWithCounts>> Handle(GetAllProducerOffersQuery request, CancellationToken cancellationToken)
        {
            int AllCount, CompletedCount, ClosedCount, PendingCount = ClosedCount = CompletedCount = AllCount = 0;
            var offer = _context.ProducerCustomerOffers
                .Select(po => new {OfferId = po.CustomerPublishedOffer.ID, OfferName = po.CustomerPublishedOffer.Name, ProducerFName = po.Producer.FirstName, ProducerLName = po.Producer.LastName, CustomerFName = po.CustomerPublishedOffer.Customer.FirstName, CustomerLName = po.CustomerPublishedOffer.Customer.LastName,
                    ProducerProfileImage = po.Producer.ProfileImageUrl, CustomerProfileImage = po.CustomerPublishedOffer.Customer.ProfileImageUrl, po.Price, po.Diposit, po.CustomerPublishedOffer.Amount, po.OfferStatus, po.CreatedAt, ChatId = po.CustomerPublishedOffer.ActiveOfferLogs,
                    po.Steps, DesignImages = po.CustomerPublishedOffer.CustomerDesign.DesignImages.Select(im=>im.ImageUrl), DesignId = po.CustomerPublishedOffer.CustomerDesign.ID });

            if (request.OfferName != null)
                offer = offer.Where(o => o.OfferName.Contains(request.OfferName));

            var counts = offer.GroupBy(_ => 1).Select(o => new
            {
                AllCount = o.Count(),
                CompletedCount = o.Count(c => c.OfferStatus == OfferStatus.Accepted),
                ClosedCount = o.Count(c => c.OfferStatus == OfferStatus.Declined),
                PendingCount = o.Count(c => c.OfferStatus == OfferStatus.OnHold)
            }).FirstOrDefault();

            if (request.IsCompleted)
                offer = offer.Where(o => o.OfferStatus == OfferStatus.Completed);
            else if(request.IsAccepted)
                offer = offer.Where(po => po.OfferStatus == OfferStatus.Accepted);

            else if (request.IsPending)
                offer = offer.Where(po => po.OfferStatus == OfferStatus.OnHold);
            else if (request.IsCLosed)
                offer = offer.Where(po => po.OfferStatus == OfferStatus.Declined);

            offer = offer.OrderByDescending(po => po.CreatedAt);
            if (!request.Newest)
                offer = offer.Reverse();

            request.PageNum = Math.Min(1,request.PageNum);
            request.PageSize = Math.Min(6, request.PageSize);

            offer = offer.Skip((request.PageNum-1)*request.PageSize);

            var responses = new CollaborationsWithCounts
            {
                AllCount = counts.AllCount,
                CompletedCount = counts.CompletedCount,
                ClosedCount = counts.ClosedCount,
                PendingCount = counts.PendingCount,
                collaborationResponses = new List<CollaborationResponse>()
            };

            foreach(var o in offer)
            {
                responses.collaborationResponses.Add(new CollaborationResponse
                {
                    DesignId = o.DesignId,
                    DesignImages = o.DesignImages.ToList(),
                    PublishOfferName = o.OfferName,
                    CustomerName = o.CustomerFName + ' ' + o.CustomerLName,
                    ProducerName = o.ProducerFName + ' ' + o.ProducerLName,
                    CustomerImage = o.CustomerProfileImage,
                    ProducerImage = o.ProducerProfileImage,
                    Deposit = o.Diposit,
                    Amount = o.Amount,
                    Steps = o.Steps.ToDictionary(s => s.StepName, d => new Tuple<int, int>(d.MinDuration, d.MaxDuration)),
                    CreatedAt = o.CreatedAt,
                    ChatId = o.ChatId.Count() == 0 ? 0: o.ChatId.First().ChatID,
                    Status = o.OfferStatus.ToString()
                });
            }
            return responses;
        }
    }
}
