using Application.Response;
using MediatR;

namespace Application.Commands.OffersFeature
{
    public class AcceptOfferCommand:IRequest<Result<int>>
    {
        public Guid ProducerOfferId { get; set; }
        public decimal Amount { get; set; }

    }
}
