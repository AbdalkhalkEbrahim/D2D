using Application.Response;
using Domain.DTOs.NotificationDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.NotificationFeature
{
    public class ReadNotificationCommand:IRequest<Result<NotificationResponse>>
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
    }
}
