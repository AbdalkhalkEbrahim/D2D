using Application.Commands.Admin;
using Application.Response;
using Domain.Enums.Status;
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
    public class SoftDeleteUserCommandHandler : IRequestHandler<SoftDeleteUserCommand, Result>
    {
        private readonly D2DContext _context;
        public SoftDeleteUserCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            var deletedUser = await _context.Users
              .Where(u => u.Id == request.UserId && !u.IsDeleted)
              .ExecuteUpdateAsync(setters => setters
                  .SetProperty(u => u.IsDeleted, true)
                  .SetProperty(u => u.IdentityStatus, VerificationStatus.Suspended));
            if (deletedUser == 0)
                return Result.Failure(Messages.NotFound.WithTarget("User"));
            return Result.Success();
        }
        
    }
}
