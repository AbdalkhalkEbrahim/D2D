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
            var chat = await _context.Chats.AsNoTracking().Select(ch=>new {ch.ID, AllMessages = ch.Messages, pName = ch.Producer.AnonName, cName = ch.Customer.AnonName, ch.ProducerID, ch.CustomerID}).FirstOrDefaultAsync(c => c.ID == request.ChatId);
            if(chat == null)
                return Result<ChatWithMessagesResponse>.Failure(Messages.NotFound.WithTarget("Chat"));
            if(chat.AllMessages.OrderByDescending(m=>m.CreatedAt).First().Sender.ToString() != request.UserType.ToString())
            {
                var message = new Message { ID = chat.AllMessages.OrderByDescending(m=>m.CreatedAt).First().ID,Content = chat.AllMessages.OrderByDescending(m=>m.CreatedAt).First().Content, IsRead = true };
                _context.Attach(message);
                _context.Entry(message).Property(m => m.IsRead).IsModified = true;
                await _context.SaveChangesAsync();
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
