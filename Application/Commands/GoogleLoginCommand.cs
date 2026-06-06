using Domain.DTOs;
using Domain.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands
{
    public class GoogleLoginCommand: IRequest<JwtToken>
    {
        [Required]
        public string IdToken { get; set; }
/*        public UserType UserType { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }*/
    }
}
