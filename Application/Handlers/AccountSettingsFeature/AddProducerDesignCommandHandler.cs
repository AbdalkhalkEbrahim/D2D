using Application.Commands.AccountSettingsFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Designs;
using Domain.Entities.Producers;
using Domain.Enums.Types;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.AccountSettingsFeature
{
    public class AddProducerDesignCommandHandler : IRequestHandler<AddProducerDesignCommand, Result<Guid>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;
        private readonly IModelesService _modelesService;
        private string prompt = "You are a strict fashion design classifier. Analyze the provided image and determine how\r\nclosely it relates to fashion designs, clothing items, apparel manufacturing, streetwear patterns, technical clothing sketches, or wearable garments.\r\n\r\nTask:\r\nEvaluate the image and output a single decimal number between 0.00 and 1.00 indicating the confidence score:\r\n\r\n1.00: The image is explicitly a fashion design, clothing item, apparel catalog photo, or garment sketch.\r\n\r\n0.00: The image has absolutely nothing to do with fashion, clothes, or apparel (e.g., cars, nature, animals, generic electronics).\r\n\r\nStrict Output Constraint:\r\nYou MUST return ONLY the raw decimal number (e.g., 0.95 or 0.15). Do NOT include any markdown blocks, no JSON formatting, no introductory phrases, and no explanations. Output the bare number only.";

        public AddProducerDesignCommandHandler(D2DContext context, IUploadService uploadService,IModelesService modelesService)
        {
            _context = context;
            _uploadService = uploadService;
            _modelesService = modelesService;
        }
        public async Task<Result<Guid>> Handle(AddProducerDesignCommand request, CancellationToken cancellationToken)
        {

            var producer = await _context.Producers.FirstOrDefaultAsync(p => p.Id == request.ProducerId&& !p.IsDeleted);
            if (producer == null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("User"));
            foreach (var d in request.Images)
            {

                if (producer.ProducerDesigns.Any(design => design.Name == request.Name && design.ProducerID == request.ProducerId))
                    return Result<Guid>.Failure(new Error("Conflict", $"There is an already design with name {request.Name}, change it then try to save again"));
                var score = await _modelesService.AnalaysisImageScore(prompt, d);

                if (!score.IsSuccess || score.Value <= 0.60m)
                    return Result<Guid>.Failure(new Error("BadRequest", "The content uploaded violates our polices, please try to upload again more suitable content"));
            }
            //check for uniqueness
            var gallery = new ProducerDesign { ProducerID = request.ProducerId, Notes = request.Description,
                Category = request.Category, Location = request.Location, Name = request.Name, DesignType = DesignType.ProducerGallery  };

            _context.Add(gallery);

            foreach (var d in request.Images)
            {
                var designToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { d });
                var designImage = new DesignImage { ProducerDesignID = gallery.ID };

                BackgroundJob.Enqueue<IUploadService>(uploadService =>
                       uploadService.UploadAndSaveSingleFile(designImage, "ImageUrl", designToBeUploaded[0], false));
            }

            await _context.SaveChangesAsync();

            return gallery.ID;
        }
    }
}
