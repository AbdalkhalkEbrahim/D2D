using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class AddBalanceCommand:IRequest<Result<int>>
    {
        public string UserId { get; set; }
        public int Amount { get; set; }

    }
}
