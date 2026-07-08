using Application.Queries.Admin;
using Application.Response;
using Domain.DTOs.Admin;
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
    public class GetUsersCountQueryHandler : IRequestHandler<GetUsersCountQuery, Result<UserCountResponse>>
    {
        private readonly D2DContext _context;
        public GetUsersCountQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<UserCountResponse>> Handle(GetUsersCountQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var lastMonth = now.AddMonths(-1);
            var twoMonthsAgo = now.AddMonths(-2);

            var counts = await _context.Users
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    LastMonthProducers = g.Count(u =>
                        u.UserType == UserType.Producer &&
                        u.JoinDate >= lastMonth),

                    LastMonthCustomers = g.Count(u =>
                        u.UserType == UserType.Customer &&
                        u.JoinDate >= lastMonth),

                    PreviousMonthProducers = g.Count(u =>
                        u.UserType == UserType.Producer &&
                        u.JoinDate >= twoMonthsAgo &&
                        u.JoinDate < lastMonth),

                    PreviousMonthCustomers = g.Count(u =>
                        u.UserType == UserType.Customer &&
                        u.JoinDate >= twoMonthsAgo &&
                        u.JoinDate < lastMonth)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (counts == null)
                return new UserCountResponse();

            double customerPercentage = counts.PreviousMonthCustomers == 0
                ? (counts.LastMonthCustomers > 0 ? 100 : 0)
                : ((counts.LastMonthCustomers - counts.PreviousMonthCustomers) * 100.0)
                    / counts.PreviousMonthCustomers;

            double producerPercentage = counts.PreviousMonthProducers == 0
                ? (counts.LastMonthProducers > 0 ? 100 : 0)
                : ((counts.LastMonthProducers - counts.PreviousMonthProducers) * 100.0)
                    / counts.PreviousMonthProducers;

            return new UserCountResponse
            {
                CustomerCount = counts.LastMonthCustomers,
                ProducerCount = counts.LastMonthProducers,
                CustomerPercentage =Math.Round( customerPercentage),
                ProducerPercentage =Math.Round( producerPercentage)
            };
        }
    }
}
