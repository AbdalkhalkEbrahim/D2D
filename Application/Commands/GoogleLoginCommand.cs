using Domain.DTOs;
using Domain.Enums;
using MediatR;

namespace Application.Commands
{
    public class GoogleLoginCommand: IRequest<JwtToken>
    {
        public string IdToken { get; set; }
        public UserType UserType { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
