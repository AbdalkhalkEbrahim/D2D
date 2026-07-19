using Application.Queries.ChatFeature;
using Application.Response;
using Domain.DTOs.Chat;
using Domain.Enums.Status;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenAI.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.ChatFeature
{
    public class GetAllChatsQueryHandler : IRequestHandler<GetAllChatsQuery, Result<List<ChatsResponse>>>
    {
        private readonly D2DContext _context;

        public GetAllChatsQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<ChatsResponse>>> Handle(GetAllChatsQuery request, CancellationToken cancellationToken)
        {

            var chats = _context.Chats
             .Where(ch => (ch.CustomerID == request.UserId || ch.ProducerID == request.UserId) && !ch.IsClosed)
             .Select(ch => new
             {
                 ChatId = ch.ID,
                 AnonName = request.Type == UserType.Customer ? ch.Producer.AnonName : ch.Customer.AnonName,
                 LastMessageInfo = ch.Messages
                     .OrderByDescending(m => m.CreatedAt)
                     .Select(m => new
                     {
                         m.Content,
                         m.CreatedAt,
                         m.IsRead,
                         IsSenderMe = m.Sender.ToString() == request.Type.ToString()
                     })
                     .FirstOrDefault(),
                 OfferStatus = _context.ActiveOfferLogs
                     .Where(ao => ao.ChatID == ch.ID)
                     .OrderByDescending(ao => ao.CreatedAt)
                     .Select(ao => ao.Step)
                     .FirstOrDefault(),
                 DesignImageUrl = ch.Customer.Designs
                     .Where(d => d.Status == DesignStatus.Published && d.CustomerPublishedOffer.IsActive)
                     .Select(d => d.DesignImages.Select(img => img.ImageUrl).FirstOrDefault())
                     .FirstOrDefault(),
                     ch.CreatedAt
             });

            if (!chats.Any() || chats.All(ch=>ch.OfferStatus == ActiveOfferStatus.Canceled.ToString()))
            {
                return new List<ChatsResponse> { };
            }

            if (request.Unread)
            {
                chats = chats.Where(ch => !ch.LastMessageInfo.IsRead && ch.OfferStatus != ActiveOfferStatus.Canceled.ToString());
            }

            else if (request.Completed)
            {
                chats = chats.Where(ch => ch.OfferStatus == ActiveOfferStatus.Completed.ToString() && ch.OfferStatus != ActiveOfferStatus.Canceled.ToString());
            }

            chats.OrderByDescending(ch => ch.LastMessageInfo.CreatedAt);
            
            var response = chats.Select(ch => new ChatsResponse
            {
                ChatId = ch.ChatId,
                DesignImageUrl = ch.DesignImageUrl,
                AnonName = ch.AnonName,
                LastMessageAgo = ch.LastMessageInfo == null? default: ch.LastMessageInfo.CreatedAt,
                LastMessage = ch.LastMessageInfo == null? null : ch.LastMessageInfo.Content.Last(),
                OfferStatus = ch.OfferStatus,
                IsRead = ch.LastMessageInfo != null && ch.LastMessageInfo.IsRead
            }).ToList();

            return response;
        }
    }
}
