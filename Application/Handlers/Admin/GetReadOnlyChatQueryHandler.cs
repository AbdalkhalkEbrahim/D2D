using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs;
using Domain.DTOs.Admin;
using Domain.Enums.Types;
using Google.Apis.Util;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Admin
{
    public class GetReadOnlyChatQueryHandler : IRequestHandler<GetReadOnlyChatQuery, Result<ReadOnlyChatResponse>>
    {
        private readonly D2DContext _context;

        public GetReadOnlyChatQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ReadOnlyChatResponse>> Handle(GetReadOnlyChatQuery request, CancellationToken cancellationToken)
        {
            var chatResponse = await _context.ActiveOfferLogs
                .AsNoTracking()
                .Where(c => c.ChatID == request.ChatId)
                .Select(c => new ReadOnlyChatResponse
                {
                    ChatId = c.ChatID,
                    OfferName = c.CustomerPublishedOffer.CustomerDesign.Name,
                    PublishedOfferId = (Guid)c.PublishedOfferID,
                    DesignImages = c.CustomerPublishedOffer.CustomerDesign.DesignImages.Select(im => im.ImageUrl).ToList(),
                    Messages = c.Chat.Messages.Select(m => new ReadOnlyMessagesResponse
                    {
                        Content = m.Content,
                        SentAt = m.CreatedAt,
                        ProfileImage = m.Sender == MessageSender.Customer ? c.Chat.Customer.ProfileImageUrl
                        : c.Chat.Producer.ProfileImageUrl,

                        Name = m.Sender == MessageSender.Customer ? c.Chat.Customer.FirstName + " " + c.Chat.Customer.LastName
                        : c.Chat.Producer.FirstName + " " + c.Chat.Producer.LastName
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (chatResponse == null)
                return Result<ReadOnlyChatResponse>.Failure(Messages.NotFound.WithTarget("Chat"));

            return chatResponse;
        }
    }
}
