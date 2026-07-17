using Application.Commands.DesignFeature;
using Application.Interfaces;
using Application.Response;
using Domain.Entities.Designs;
using Hangfire;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MimeKit.Cryptography;
using OpenAI.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.DesignFeature
{
    public class SaveDesignCommandHandler : IRequestHandler<SaveDesignCommand, Result<List<Guid>>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;

        public SaveDesignCommandHandler(D2DContext context, IUploadService uploadService)
        {
            _context = context;    
            _uploadService = uploadService;
        }

        public async Task<Result<List<Guid>>> Handle(SaveDesignCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Include(c=>c.Designs).FirstOrDefaultAsync(c => c.Id == request.Id);
            if (customer == null)
                return Result<List<Guid>>.Failure(Messages.NotFound.WithTarget("User"));

            

            var Ids = new List<Guid>();
            foreach (var d in request.DesignsSaved)
            {
                if (customer.Designs.Any(design => design.Name == d.Name))
                    return Result<List<Guid>>.Failure(new Error("Conflict", $"There is an already design with name {d.Name}, change it then try to dave again"));
                var design = new CustomerDesign
                {
                    Name = d.Name,
                    CustomerId = customer.Id,
                    Notes = d.Notes,
                    
                };

                await _context.AddAsync(design);
                await _context.SaveChangesAsync();

                Ids.Add(design.ID);
                var designToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { d.DesignImage });

                var designImage = new DesignImage { CustomerDesignID = design.ID };
                BackgroundJob.Enqueue<IUploadService>(uploadService =>
                    uploadService.UploadAndSaveSingleFile(designImage, "ImageUrl", designToBeUploaded[0], false));
            }
            
            return Ids;
        }
    }
}
