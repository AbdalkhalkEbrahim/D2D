using Application.Response;
using Domain.Enums.Status;
using MediatR;

namespace Application.Commands.ChatFeature
{
    public class ChangeActiveOfferStatusCommand : IRequest<Result<string>>
    {
        public int ChatID { get; set; } 
        public string Step { get; set; }
    }
}
