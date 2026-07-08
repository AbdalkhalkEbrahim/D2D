using Application.Commands.ChatFeature;
using Application.Response;
using Domain.DTOs.Chat;
using Domain.Entities.Chats;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.ChatFeature
{
    public class GetChatMessagesCommandHandler : IRequestHandler<GetChatMessagesCommand, Result<ChatWithMessagesResponse>>
    {
        private readonly D2DContext _context;

        public GetChatMessagesCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ChatWithMessagesResponse>> Handle(GetChatMessagesCommand request, CancellationToken cancellationToken)
        {
            var chat =  _context.Chats.Select(ch=>new {ch.ID, AllMessages = ch.Messages, pName = ch.Producer.AnonName, cName = ch.Customer.AnonName, ch.ProducerID, ch.CustomerID}).Where(c => c.ID == request.ChatId);
            if(!chat.Any() )
                return Result<ChatWithMessagesResponse>.Failure(Messages.NotFound.WithTarget("Chat"));
            if(chat.First().AllMessages.First().Sender.ToString() != request.UserType.ToString())
            {
               await chat.ExecuteUpdateAsync(s => s
                .SetProperty(c => c.AllMessages.First().IsRead, true));  
            }
            var chatWithMessages = await chat.FirstOrDefaultAsync(cancellationToken);
            var response = new ChatWithMessagesResponse
            {
                Messages = chatWithMessages.AllMessages.Select(m => new MessagesResponse
                {
                    ID = m.ID,
                    Message = m.Content,
                    Sender = m.Sender,
                    IsRead = m.IsRead,
                    CreatedAt = m.CreatedAt
                }).OrderByDescending(m => m.CreatedAt).ToList(),
                AnonName = request.UserType == UserType.Customer ? chatWithMessages.pName : chatWithMessages.cName,
                OtherId = request.UserType == UserType.Customer ? chatWithMessages.ProducerID : chatWithMessages.CustomerID
            };

            return response;

        }

        
    }
}
