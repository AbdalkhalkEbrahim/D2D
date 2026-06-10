using Application.Response;
using Domain.DTOs;
using MediatR;

namespace Application.Commands
{
    public class VerifyMagicTokenCommand:IRequest<Result<JwtToken>>
    {
        public string Token { get; set; }   
    }
}
