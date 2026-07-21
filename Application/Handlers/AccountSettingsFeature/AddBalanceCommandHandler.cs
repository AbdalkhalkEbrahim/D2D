using Application.Commands.AccountSettingsFeature;
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
using Domain.Entities.Payment;
using Domain.Enums.Status;

namespace Application.Handlers.AccountSettingsFeature
{
    public class AddBalanceCommandHandler : IRequestHandler<AddBalanceCommand, Result<decimal>>
    {
        private readonly D2DContext _context;

        public AddBalanceCommandHandler(D2DContext context)
        {
            _context = context;   
        }
        public async Task<Result<decimal>> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == request.UserId && !u.IsDeleted, cancellationToken);

            if (user == null)
                return Result<decimal>.Failure(Messages.NotFound.WithTarget("User"));

            if (request.Amount <= 0)
                return Result<decimal>.Failure(Messages.Conflict.WithTarget("Balance"));

            if (request.UserType == UserType.Customer.ToString() && user is Customer customer)
            {
                customer.Balance += request.Amount;
                _context.Update(customer);
            }
            else if (request.UserType == UserType.Producer.ToString() && user is Producer producer)
            {
                producer.Balance += request.Amount;
                _context.Update(producer);

            }
            var trans = new Transaction { Amount = request.Amount, CreatedAt = DateTime.UtcNow, Type = TransactionType.Deposit, UserID =  request.UserId, Currency = "eg" };
            _context.Add(trans);
            await _context.SaveChangesAsync(cancellationToken);

            return user.Balance;
        }
    }
}
