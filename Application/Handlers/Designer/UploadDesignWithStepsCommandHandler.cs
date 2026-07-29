using Application.Commands.Designer;
using Application.Interfaces;
using Application.Response;
using Domain.DTOs.Designer;
using Domain.Entities.Designers;
using Domain.Enums.Status;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.Designer
{
    public class UploadDesignWithStepsCommandHandler : IRequestHandler<UploadDesignWithStepsCommand, Result<UploadDesignResponse>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;

        public UploadDesignWithStepsCommandHandler(D2DContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }
        public async Task<Result<UploadDesignResponse>> Handle(UploadDesignWithStepsCommand request, CancellationToken cancellationToken)
        {
            var designer = await _context.Designers.AsNoTracking().FirstOrDefaultAsync(d => d.Id == request.DesignerId);
            if (designer == null)
                return Result<UploadDesignResponse>.Failure(Messages.NotFound.WithTarget("Designer"));
            var designToBeUploaded = await _uploadService.ChangeFileFormat(request.Design);
            var stepsToBeUploaded = await _uploadService.ChangeFileFormat(request.Steps);

            var design = new DesignVerification
            {
                DesignerID = request.DesignerId,
                Status = VerificationStatus.Pending
            };

            _context.Add(design);
            await _context.SaveChangesAsync();

            BackgroundJob.Enqueue<IUploadService>(uploadService =>
                      uploadService.UploadAndSaveSingleFile(design, "FinalDesign", designToBeUploaded, false));

            foreach (var step in stepsToBeUploaded)
            {
                BackgroundJob.Enqueue<IUploadService>(uploadService =>
                       uploadService.UploadAndSaveSingleFile(design, "StepUrl", step, false));
            }
            return new UploadDesignResponse
            {
                DesignId = design.DesignId,
                Status = VerificationStatus.Pending
            };
        }
    }
}
