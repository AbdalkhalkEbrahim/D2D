using Application.Response;
using Domain.DTOs;
using MediatR;
namespace Application.Commands
{
    public class SendOtpCommand : IRequest<Result<OtpResponse>>
    {
        public required string ID { get; set; }
    }
}
