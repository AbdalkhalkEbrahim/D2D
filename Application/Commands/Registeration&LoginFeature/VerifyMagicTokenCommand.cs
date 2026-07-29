using Application.Response;
using Domain.DTOs.AuthDtos;
using MediatR;

namespace Application.Commands.RegisterationFeature
{
    public class VerifyMagicTokenCommand : IRequest<Result<JwtToken>>
    {
        public string Token { get; set; }
    }
}
