using Application.Queries.AccountSettings;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.Entities.Shared;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.AccountSettingsFeature
{
    public class GetAllGalleryQueryHandler : IRequestHandler<GetAllGalleryQuery, Result<List<GalleryResponse>>>
    {
        private readonly D2DContext _context;

        public GetAllGalleryQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<List<GalleryResponse>>> Handle(GetAllGalleryQuery request, CancellationToken cancellationToken)
        {
            var producer = await _context.Producers.Select(p => new { p.Id, p.IsDeleted,
                Gallery = p.ProducerDesigns.Where(g=>!g.IsDeleted).Select(g=> new {g.ID, g.IsDeleted, g.Notes, g.Category, g.CreatedAt, g.UpdatedAt, g.Location, g.Name,
                    Images = g.DesignImages.Select(di=> new { di.ID, di.ImageUrl }).ToList()}) })
                .FirstOrDefaultAsync(p => p.Id == request.ProducerId && !p.IsDeleted);

            if (producer == null)
                return Result<List<GalleryResponse>>.Failure(Messages.NotFound.WithTarget("User"));

            var gallery = producer.Gallery.ToList();
            
            var response = producer.Gallery.GroupBy(g => g.Name)
                .ToDictionary(
                    g => g.Key ?? string.Empty,
                    g => g.SelectMany(x => x.Images).ToList()
                );
            var finalResponse = new List<GalleryResponse>();
            foreach ( var item in gallery)
            {
                finalResponse.Add(new GalleryResponse
                {
                    DesignId = item.ID,
                    Images = item.Images.Select(im=> new Tuple<int,string>(im.ID,im.ImageUrl)).ToList(),
                    Notes = item.Notes,
                    Location = item.Location,
                    Category = item.Category,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                });
            }
            return finalResponse;
        }
    }
}
