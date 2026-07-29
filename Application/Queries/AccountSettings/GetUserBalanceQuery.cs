using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.AccountSettings
{
    public class GetUserBalanceQuery:IRequest<Result<decimal>>
    {
        public string UserId { get; set; }
    }
}
