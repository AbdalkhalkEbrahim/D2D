using Application.Response;
using Domain.DTOs.Admin;
using MediatR;

namespace Application.Queries.Admin
{
    public class GetReadOnlyChatQuery:IRequest<Result<ReadOnlyChatResponse>>
    {
        public int ChatId { get; set; }
    }
}
