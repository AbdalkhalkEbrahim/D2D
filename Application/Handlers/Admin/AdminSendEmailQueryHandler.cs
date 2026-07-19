using Application.Interfaces;
using Application.Queries.Admin;
using Application.Response;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Signers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Admin
{
    public class AdminSendEmailQueryHandler : IRequestHandler<AdminSendEmailQuery, Result>
    {
        private readonly D2DContext _context;

        public AdminSendEmailQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(AdminSendEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(u=>!u.IsDeleted).Select(u => new { u.Id, u.Email }).FirstOrDefaultAsync(u => u.Id == request.UserId);
            if(user == null)
                return Result.Failure(Messages.BadRequest.WithTarget("Usser"));

            BackgroundJob.Enqueue<IEmailService>(emailService =>
               emailService.SendEmailAsync(user.Email, request.EmailTitle, request.EmailContent));

            return Result.Success();
        }
    }
}
