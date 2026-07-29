using Application.Commands.DesignFeature;
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

namespace Application.Handlers.DesignFeature
{
    public class DeleteDesignCommandHandler : IRequestHandler<DeleteDesignCommand, Result>
    {
        private readonly D2DContext _context;

        public DeleteDesignCommandHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteDesignCommand request, CancellationToken cancellationToken)
        {

            var deletedDesign = await _context.CustomerDesigns.Where(d => d.ID == request.Id && d.Status == DesignStatus.Drafted).ExecuteDeleteAsync();
            if (deletedDesign == 0)
                return Result.Failure(Messages.BadRequest.WithTarget("Design"));

            return Result.Success();
        }
    }
}
