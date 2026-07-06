using Application.Response;
using Domain.Enums.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatFeature
{
    public class SendRequestCommand:IRequest<Result<bool>>
    {
        public CancelationRequest request { get; set; }
        public int ChatId { get; set; }
        public string UserId { get; set; }
    }
}
