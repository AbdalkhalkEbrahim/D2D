using Application.Commands.AccountSettingsFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Producers;
using Hangfire;
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
    public class PushToProducerGalleryCommandHandler : IRequestHandler<PushToProducerGalleryCommand, Result>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;
        public PushToProducerGalleryCommandHandler(D2DContext context, IUploadService uploadService)
        {
            _context = context;
            _uploadService = uploadService;
        }
        public async Task<Result> Handle(PushToProducerGalleryCommand request, CancellationToken cancellationToken)
        {

            var producer = await _context.Producers.FirstOrDefaultAsync(p => p.Id == request.ProducerId);
            if (producer == null)
                return Result.Failure(Messages.NotFound.WithTarget("User"));

            //check for uniqueness
            var gallery = new ProducerGallery { ProducerId = request.ProducerId, Description = request.Description };

            var filesToBeUploaded = await _uploadService.ChangeFileFormat(request.Images);
            foreach (var image in filesToBeUploaded) 
                BackgroundJob.Enqueue<IUploadService>(uploadService => uploadService.UploadAndSaveSingleFile(gallery, "ImageUrl", image, true));

            return Result.Success();
        }
    }
}
