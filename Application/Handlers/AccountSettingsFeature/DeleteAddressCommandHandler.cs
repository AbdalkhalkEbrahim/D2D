using Application.Commands.AccountSettingsFeature;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
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
    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Result>
    {
        private readonly D2DContext _context;
        public DeleteAddressCommandHandler(D2DContext context)
        {
            _context = context;
        }

        public async Task<Result> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            //var deletedaddress = await _context.Customers.SelectMany(c=>c.Addresses).Where(a => a.ID == request.Id&&!a.Selected).ExecuteDeleteAsync();
            //if (deletedaddress == 0)
            //    return Result.Failure(Messages.NotFound.WithTarget("Address"));
            var user = await _context.Customers.FirstOrDefaultAsync(c =>!c.IsDeleted&& c.Addresses.Any(a => a.ID == request.Id));
            if (user == null)
                return Result.Failure(Messages.NotFound.WithTarget("User"));

            var address = user.Addresses.FirstOrDefault(a => a.ID == request.Id);

            if (address == null)
                return Result.Failure(Messages.NotFound.WithTarget("Address"));
            if (address.Selected)
                return Result.Failure(Messages.Conflict.WithTarget("Address"));

            _context.Remove(address);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
