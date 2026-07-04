using Application.Response;
using Domain.DTOs.Chat;
using Domain.Enums.Types;
using MediatR;

namespace Application.Commands.ChatFeature
{
    public class GetChatMessagesCommand:IRequest<Result<ChatWithMessagesResponse>>
    {
        public int ChatId { get; set; }
        public UserType UserType { get; set; }  
    }
}
