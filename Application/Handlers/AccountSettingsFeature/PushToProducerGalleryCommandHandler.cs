using Application.Commands.AccountSettingsFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Producers;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class PushToProducerGalleryCommandHandler : IRequestHandler<PushToProducerGalleryCommand, Result>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;
        private readonly IModelesService _modelesService;
        private string prompt = "You are a strict fashion design classifier. Analyze the provided image and determine how\r\nclosely it relates to fashion designs, clothing items, apparel manufacturing, streetwear patterns, technical clothing sketches, or wearable garments.\r\n\r\nTask:\r\nEvaluate the image and output a single decimal number between 0.00 and 1.00 indicating the confidence score:\r\n\r\n1.00: The image is explicitly a fashion design, clothing item, apparel catalog photo, or garment sketch.\r\n\r\n0.00: The image has absolutely nothing to do with fashion, clothes, or apparel (e.g., cars, nature, animals, generic electronics).\r\n\r\nStrict Output Constraint:\r\nYou MUST return ONLY the raw decimal number (e.g., 0.95 or 0.15). Do NOT include any markdown blocks, no JSON formatting, no introductory phrases, and no explanations. Output the bare number only.";

        public PushToProducerGalleryCommandHandler(D2DContext context, IUploadService uploadService,IModelesService modelesService)
        {
            _context = context;
            _uploadService = uploadService;
            _modelesService = modelesService;
        }
        public async Task<Result> Handle(PushToProducerGalleryCommand request, CancellationToken cancellationToken)
        {

            var producer = await _context.Producers.FirstOrDefaultAsync(p => p.Id == request.ProducerId);
            if (producer == null)
                return Result.Failure(Messages.NotFound.WithTarget("User"));
            foreach (var img in request.Images)
            {
                var score = await _modelesService.AnalaysisImageScore(prompt, img);

                if (!score.IsSuccess || score.Value <= 0.75m)
                    return Result.Failure(Messages.BadRequest.WithTarget("ValidationError"));
            }
            //check for uniqueness
            var gallery = new ProducerGallery { ProducerId = request.ProducerId, Description = request.Description };

            var filesToBeUploaded = await _uploadService.ChangeFileFormat(request.Images);
            foreach (var image in filesToBeUploaded) 
                BackgroundJob.Enqueue<IUploadService>(uploadService => uploadService.UploadAndSaveSingleFile(gallery, "ImageUrl", image, true));

            return Result.Success();
        }
    }
}
