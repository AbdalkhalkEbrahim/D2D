using Application.Commands.Admin;
using Application.Response;
using Domain.Entities.Customers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Admin
{
    public class ChangeUserStatusCommandHandler : IRequestHandler<ChangeUserStatusCommand, Result>
    {
        private readonly D2DContext _context;
        public ChangeUserStatusCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
        {
            var user = _context.Users.Where(c => c.Id == request.Id);
            if(!user.Any())
                return Result.Failure(Messages.NotFound.WithTarget("User"));

           await user.ExecuteUpdateAsync(s => s
                .SetProperty(c => c.IdentityStatus, request.Status));
            return Result.Success();
        }
    }
}
