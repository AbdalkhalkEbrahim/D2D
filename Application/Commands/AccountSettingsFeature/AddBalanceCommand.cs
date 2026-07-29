using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class AddBalanceCommand:IRequest<Result<decimal>>
    {
        public string UserId { get; set; }
        public string UserType { get; set; }
        public decimal Amount { get; set; }

    }
}
