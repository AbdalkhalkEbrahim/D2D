using Application.Commands;
using Application.Commands.AccountSettingsFeature;
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
    public class ChangeEmailCommandHandler : IRequestHandler<ChangeEmailCommand, Result<string>>
    {
        private readonly D2DContext _context;
        private readonly IMediator _mediato;

        public ChangeEmailCommandHandler(D2DContext context, IMediator mediator)
        {
            _context = context;
            _mediato = mediator;
        }
        public async Task<Result<string>> Handle(ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediato.Send(new VerifyOtpCommand { UserId = request.Id, Otp = request.Otp, flag = false });
            if (!result.IsSuccess)
                return Result<string>.Failure(result.Error);

            await _context.Users.Where(u => u.Id == request.Id).ExecuteUpdateAsync(setter => setter.SetProperty(u => u.Email, request.Email));
            await _context.SaveChangesAsync();
            return request.Email;
        }
    }
}
