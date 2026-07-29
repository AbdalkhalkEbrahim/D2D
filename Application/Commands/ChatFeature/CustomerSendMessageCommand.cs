using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatFeature
{
    public class CustomerSendMessageCommand:IRequest<Result>
    {
        public int ChatId { get; set; }
        public string MessageText { get; set; }
    }
}
