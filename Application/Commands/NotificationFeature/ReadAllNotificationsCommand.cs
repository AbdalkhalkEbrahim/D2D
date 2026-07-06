using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.NotificationFeature
{
    public class ReadAllNotificationsCommand: IRequest<Result>
    {
        public string UserId { get; set; }
    }
}
