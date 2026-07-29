using Application.Response;
using Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatFeature
{
    public class SendOfferOtpCommand : IRequest<Result<OtpResponse>>
    {
        public string CustomerId { get; set; }
        public string ProducerId { get; set; }
    }
}
