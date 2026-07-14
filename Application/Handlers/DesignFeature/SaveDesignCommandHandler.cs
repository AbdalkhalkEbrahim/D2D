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

            if (customer.Designs.Any(d => d.Name == request.Name))
                return Result<List<Guid>>.Failure(Messages.Conflict.WithTarget("Design"));

            var designToBeUploaded = await _uploadService.ChangeFileFormat(request.DesignImage);
            var Ids = new List<Guid>();
            foreach (var d in designToBeUploaded)
            {

                var design = new CustomerDesign
                {
                    Name = request.Name,
                    CustomerId = customer.Id,
                };

                await _context.AddAsync(design);
                await _context.SaveChangesAsync();

                Ids.Add(design.ID);

                var designImage = new DesignImage { CustomerDesignID = design.ID };
                BackgroundJob.Enqueue<IUploadService>(uploadService =>
                    uploadService.UploadAndSaveSingleFile(designImage, "ImageUrl", d, false));
            }
            
            return Ids;
        }
    }
}
