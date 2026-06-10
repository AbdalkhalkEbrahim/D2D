using Application.Response;
using MediatR;

namespace Application.Commands
{
    public class SendLoginLinkCommand:IRequest<Result<string>>
    {
        public required string UserID { get; set; }
    }
}
