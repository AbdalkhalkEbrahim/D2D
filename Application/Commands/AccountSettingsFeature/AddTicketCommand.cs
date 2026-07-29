using Application.Response;
using Domain.Enums.Status;
using Domain.Enums.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class AddTicketCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string Description { get; set; }
        public IssueType IssueType { get; set; }
        public TicketStatus Status { get; set; }
    }
}
