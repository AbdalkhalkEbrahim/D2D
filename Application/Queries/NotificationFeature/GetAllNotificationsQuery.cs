using Application.Response;
using Domain.DTOs.NotificationDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.NotificationFeature
{
    public class GetAllNotificationsQuery : IRequest<Result<List<NotificationResponse>>>
    {
        public string UserId { get; set; }
    }
}
