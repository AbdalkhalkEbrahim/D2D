using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatFeature
{
    public class ProducerSendMessageCommand:IRequest<Result>
    {
        public int ChatId { get; set; }
        public string? MessageText { get; set; }
        public IFormFile? MessageImageUrl { get; set; }
    }
}
