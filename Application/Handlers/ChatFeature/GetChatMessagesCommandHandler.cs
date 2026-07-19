using Application.Commands.ChatFeature;
using Application.Response;
using Domain.DTOs.Chat;
using Domain.Entities.Chats;
using Domain.Enums.Status;
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
        public async Task<Result<ChatWithMessagesResponse>> Handle( GetChatMessagesCommand request, CancellationToken cancellationToken)
        {
            var chat = await _context.Chats.AsNoTracking().Where(c => c.ID == request.ChatId)
                .Select(c => new
                {
                    c.ID,
                    c.ProducerID,
                    c.CustomerID,
                    c.IsClosed,
                    ProducerName = c.Producer.AnonName,
                    CustomerName = c.Customer.AnonName,
                   
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (chat == null)
                return Result<ChatWithMessagesResponse>.Failure(Messages.NotFound.WithTarget("Chat"));

            var messages = await _context.Messages.Where(m => m.ChatID == request.ChatId).OrderByDescending(m => m.CreatedAt).ToListAsync(cancellationToken);

            //var selectedStep = await _context.ActiveOfferLogs.AsNoTracking().Where(al => al.ChatID == request.ChatId).OrderBy(al => al.CreatedAt)
            //    .Select(al => new
            //    {
            //        al.Step,
            //        al.CreatedAt
            //    })
            //    .FirstOrDefaultAsync (cancellationToken);
            var allStepsWithSelected = await _context.ActiveOfferLogs.Select(al => new { al.ChatID, al.Step, 
                Steps = al.CustomerPublishedOffer.ProducerCustomerOffers.
                  Where(p=>p.OfferStatus==OfferStatus.Accepted).Select(pco => pco.Steps.Select(s => s.StepName)).FirstOrDefault(),al.CreatedAt })
                
                .OrderByDescending(c=>c.CreatedAt)
                .FirstOrDefaultAsync(ch => ch.ChatID == request.ChatId);

            var lastMessage = messages.FirstOrDefault();

            if (lastMessage != null &&
                lastMessage.Sender.ToString() != request.UserType.ToString())
            {
                foreach (var message in messages.Where(m => !m.IsRead))
                {
                    message.IsRead = true;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

           // var currentStep = allStepsWithSelected.Step;
            return new ChatWithMessagesResponse
            {
                Messages = messages
                    .Select(m => new MessagesResponse
                    {
                        ID = m.ID,
                        Message = m.Content,
                        Sender = m.Sender,
                        IsRead = m.IsRead,
                        CreatedAt = m.CreatedAt
                    })
                    .ToList(),

                AnonName = request.UserType == UserType.Customer
                    ? chat.ProducerName
                    : chat.CustomerName,

                //Steps = allStepsWithSelected.Steps
                //    .ToDictionary(
                //        x => x,
                //        x => x == currentStep),

                OtherId = request.UserType == UserType.Customer
                    ? chat.ProducerID
                    : chat.CustomerID,

                IsClosed = chat.IsClosed
            };
        }


    }
}
