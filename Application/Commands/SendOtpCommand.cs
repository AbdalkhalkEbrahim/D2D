using Application.Response;
using Domain.DTOs;
using MediatR;
using System.ComponentModel.DataAnnotations;
namespace Application.Commands
{
    public class SendOtpCommand : IRequest<Result<OtpResponse>>
    {
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public required string Email { get; set; }
    }
}
