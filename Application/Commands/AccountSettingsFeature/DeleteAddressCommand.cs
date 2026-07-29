using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.AccountSettingsFeature
{
    public class DeleteAddressCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
