using Application.Commands.OffersFeature.CustomOffer;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Domain.Entities.Producers;
using Hangfire;
using Infrastructure.Data.Context;
using Infrastructure.Migrations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jsonPatch = Microsoft.AspNetCore.JsonPatch.Operations;


namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class EditProducerPublishedDesignFromGalleryCommandHandler : IRequestHandler<EditProducerPublishedDesignFromGalleryCommand, Result>
    {
        private readonly D2DContext _context;
        private readonly IModelesService _modelesService;
        //private string prompt = "You are a strict fashion design classifier. Analyze the provided image and determine how\r\nclosely it relates to fashion designs, clothing items, apparel manufacturing, streetwear patterns, technical clothing sketches, or wearable garments.\r\n\r\nTask:\r\nEvaluate the image and output a single decimal number between 0.00 and 1.00 indicating the confidence score:\r\n\r\n1.00: The image is explicitly a fashion design, clothing item, apparel catalog photo, or garment sketch.\r\n\r\n0.00: The image has absolutely nothing to do with fashion, clothes, or apparel (e.g., cars, nature, animals, generic electronics).\r\n\r\nStrict Output Constraint:\r\nYou MUST return ONLY the raw decimal number (e.g., 0.95 or 0.15). Do NOT include any markdown blocks, no JSON formatting, no introductory phrases, and no explanations. Output the bare number only.";
        private readonly IUploadService _uploadService;
        public EditProducerPublishedDesignFromGalleryCommandHandler(D2DContext context, IModelesService modelesService,IUploadService uploadService)
        {
            _context=context;
            _modelesService=modelesService;
            _uploadService=uploadService;
        }
        public async Task<Result> Handle(EditProducerPublishedDesignFromGalleryCommand request, CancellationToken cancellationToken)
        {
            var designs =  _context.ProducerDesigns.Where(d=>d.ProducerID==request.ProducerId&& !d.IsDeleted);

            if (!designs.Any())
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("Design"));

            var design = await designs.FirstOrDefaultAsync(d => d.ID == request.DesignId);

            if (request.Name != null)
            {
                if (designs.Any(design => design.Name == request.Name))
                    return Result<Guid>.Failure(new Error("Conflict", $"There is an already design with name {request.Name}, change it then try to save again"));

                design.Name = request.Name;
            }



            //if (request.Designs != null)
            //{
            //    foreach (var d in request.Designs)
            //    {
            //        var score = await _modelesService.AnalaysisImageScore(prompt, d);

            //        if (!score.IsSuccess || score.Value <= 0.75m)
            //            return Result<Guid>.Failure(new Error("BadRequest", "The content uploaded violates our polices, please try to upload again more suitable content"));
            //    }

            //    foreach (var d in request.Designs)
            //    {
            //        var designToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { d });
            //        var designImage = new DesignImage { ProducerDesignID = design.ID };

            //        BackgroundJob.Enqueue<IUploadService>(uploadService =>
            //               uploadService.UploadAndSaveSingleFile(designImage, "ImageUrl", designToBeUploaded[0], false));
            //    }
            //}

            var entityPatch = new JsonPatchDocument<ProducerDesign>();
                request.data.Operations.ForEach(op => entityPatch.Operations.Add(new jsonPatch.Operation<ProducerDesign>(op.op, op.path, op.from, op.value)));

                entityPatch.ApplyTo(design);
            
           
           
            await _context.SaveChangesAsync();

            return Result.Success();
        }
    }
}
