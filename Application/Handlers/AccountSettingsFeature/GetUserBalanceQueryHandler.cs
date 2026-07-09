using Application.Queries.AccountSettings;
using Application.Response;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.AccountSettingsFeature
{
    public class GetUserBalanceQueryHandler : IRequestHandler<GetUserBalanceQuery, Result<decimal>>
    {
        private readonly D2DContext _context;

        public GetUserBalanceQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<decimal>> Handle(GetUserBalanceQuery request, CancellationToken cancellationToken)
        {
            var userBalance = await _context.Users.Select(u => new { u.Id, u.Balance }).FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (userBalance == null)
                return Result<decimal>.Failure(Messages.NotFound.WithTarget("User"));
            return userBalance.Balance;
        }
    }
}
