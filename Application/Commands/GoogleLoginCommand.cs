using Application.Response;
using Domain.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class GoogleLoginCommand: IRequest<Result<JwtToken>>
    {
        [Required]
        public required string IdToken { get; set; }
/*        public UserType UserType { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }*/
    }
}
