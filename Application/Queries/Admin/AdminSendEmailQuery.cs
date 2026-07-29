using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Admin
{
    public class AdminSendEmailQuery:IRequest<Result>
    {
        public string UserId { get; set; }
        public string EmailTitle { get; set; }
        public string EmailContent { get; set; }
    }
}
