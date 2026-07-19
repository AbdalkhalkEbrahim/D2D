using Application.Commands.DesignFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Designers;
using Domain.Entities.Designs;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.DesignFeature
{
    public class SaveDesignCommandHandler : IRequestHandler<SaveDesignCommand, Result<List<Guid>>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;
        private readonly IModelesService _modelesService;
        private string prompt = "You are a strict fashion design classifier. Analyze the provided image and determine how\r\nclosely it relates to fashion designs, clothing items, apparel manufacturing, streetwear patterns, technical clothing sketches, or wearable garments.\r\n\r\nTask:\r\nEvaluate the image and output a single decimal number between 0.00 and 1.00 indicating the confidence score:\r\n\r\n1.00: The image is explicitly a fashion design, clothing item, apparel catalog photo, or garment sketch.\r\n\r\n0.00: The image has absolutely nothing to do with fashion, clothes, or apparel (e.g., cars, nature, animals, generic electronics).\r\n\r\nStrict Output Constraint:\r\nYou MUST return ONLY the raw decimal number (e.g., 0.95 or 0.15). Do NOT include any markdown blocks, no JSON formatting, no introductory phrases, and no explanations. Output the bare number only.";

        public SaveDesignCommandHandler(D2DContext context, IUploadService uploadService, IModelesService modelesService)
        {
            _context = context;    
            _uploadService = uploadService;
            _modelesService = modelesService;
        }

        public async Task<Result<List<Guid>>> Handle(SaveDesignCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Include(c=>c.Designs).Include(c=>c.ModelChat).FirstOrDefaultAsync(c => c.Id == request.Id);
            if (customer == null)
                return Result<List<Guid>>.Failure(Messages.NotFound.WithTarget("User"));

            foreach (var d in request.Files)
            {

                if (customer.Designs.Any(design => design.Name == request.Name && design.CustomerId == request.Id))
                    return Result<List<Guid>>.Failure(new Error("Conflict", $"There is an already design with name {request.Name}, change it then try to save again"));
                var score = await _modelesService.AnalaysisImageScore(prompt, d);

                if (!score.IsSuccess || score.Value <= 0.75m)
                    return Result<List<Guid>>.Failure(new Error("BadRequest","The content uploaded violates our polices, please try to upload again more suitable content"));
            }

            var Ids = new List<Guid>();
            var design = new CustomerDesign
            {
                Name = request.Name,
                CustomerId = customer.Id,
                Notes = request.Notes,

            };

            await _context.AddAsync(design);
            Ids.Add(design.ID);

            foreach (var d in request.Files)
            {
                var designToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { d });
                var designImage = new DesignImage { CustomerDesignID = design.ID };

                BackgroundJob.Enqueue<IUploadService>(uploadService =>
                       uploadService.UploadAndSaveSingleFile(designImage, "ImageUrl", designToBeUploaded[0], false));
            }
            await _context.SaveChangesAsync();

            return Ids;
        }
    }
}
