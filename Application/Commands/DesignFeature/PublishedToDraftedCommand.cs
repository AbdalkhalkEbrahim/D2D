using Application.Response;
using MediatR;


namespace Application.Commands.DesignFeature
{
    public class PublishedToDraftedCommand: IRequest<Result<Guid>>
    {
        public Guid DesignId { get; set; }
    }
}
