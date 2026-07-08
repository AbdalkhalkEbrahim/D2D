using Application.Commands.AccountSettingsFeature;
using Application.Response;
using Infrastructure.Data.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.AccountSettingsFeature
{
    public class AddBalanceCommandHandler : IRequestHandler<AddBalanceCommand, Result<int>>
    {
        public AddBalanceCommandHandler(D2DContext context)
        {
            
        }
        public Task<Result<int>> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
