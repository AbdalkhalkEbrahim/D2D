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
    public class SaveDesignCommandHandler : IRequestHandler<SaveDesignCommand, Result<Guid>>
    {
        private readonly D2DContext _context;
        private readonly IUploadService _uploadService;

        public SaveDesignCommandHandler(D2DContext context, IUploadService uploadService)
        {
            _context = context;    
            _uploadService = uploadService;
        }

        public async Task<Result<Guid>> Handle(SaveDesignCommand request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.Include(c=>c.Designs).FirstOrDefaultAsync(c => c.Id == request.Id);
            if (customer == null)
                return Result<Guid>.Failure(Messages.NotFound.WithTarget("User"));

            if (customer.Designs.Any(d => d.Name == request.Name))
                return Result<Guid>.Failure(Messages.Conflict.WithTarget("Design"));

            var design = new CustomerDesign
            {
                Name = request.Name,
                CustomerId = customer.Id,
            };

            await _context.AddAsync(design);
            await _context.SaveChangesAsync();

            var designImage = new DesignImage { CustomerDesignID = design.ID };

            var designToBeUploaded = await _uploadService.ChangeFileFormat(new List<IFormFile> { request.DesignImage });
            BackgroundJob.Enqueue<IUploadService>(uploadService =>
               uploadService.UploadAndSaveSingleFile(designImage, "ImageUrl", designToBeUploaded[0],false));

            
            return design.ID;
        }
    }
}
