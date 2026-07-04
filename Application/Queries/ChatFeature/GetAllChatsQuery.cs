using Application.Response;
using Domain.DTOs.Chat;
using Domain.Enums.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.ChatFeature
{
    public class GetAllChatsQuery:IRequest<Result<List<ChatsResponse>>>
    {
        public string UserId { get; set; }
        public UserType Type { get; set; }
        public bool Unread { get; set; } = false;
        public bool Completed { get; set; } = false;

    }
}
