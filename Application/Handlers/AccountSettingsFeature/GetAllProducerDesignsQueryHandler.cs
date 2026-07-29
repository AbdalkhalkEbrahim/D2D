using Application.Queries.AccountSettings;
using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using Domain.Entities.Designs;
using Domain.Entities.Shared;
using Domain.Enums.Types;
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
    public class GetAllProducerDesignsQueryHandler : IRequestHandler<GetAllProducerDesignsQuery, Result<ProducerDesignsResponse>>
    {
        private readonly D2DContext _context;

        public GetAllProducerDesignsQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<ProducerDesignsResponse>> Handle(GetAllProducerDesignsQuery request, CancellationToken cancellationToken)
        {
            var producer = await _context.Producers.Select(p => new { p.Id, p.IsDeleted, p.AnonName,
                Gallery = p.ProducerDesigns.Where(g=>!g.IsDeleted).Select(g=> new {g.ID, g.IsDeleted, g.Notes, g.Category, g.CreatedAt, g.UpdatedAt, g.Location, g.Name, Images = g.DesignImages.Select(di => new { di.ID, g.Name, di.ImageUrl }).ToList() })
            })
                .FirstOrDefaultAsync(p => p.Id == request.ProducerId && !p.IsDeleted);

            if (producer == null)
                return Result<ProducerDesignsResponse>.Failure(Messages.NotFound.WithTarget("User"));

            var gallery = producer.Gallery;

            if (request.Category != null)
               gallery = producer.Gallery.Where(g => g.Category.ToLower() == request.Category.ToLower());
            else if(request.Location != null)
                gallery = producer.Gallery.Where(g => g.Location.ToLower() == request.Location.ToLower());
            else if (request.Name != null)
                gallery = producer.Gallery.Where(g => g.Name.ToLower() == request.Name.ToLower());

            gallery = gallery.OrderBy(g => g.CreatedAt);
            if (request.Newest)
                gallery.Reverse();

            if (request.PageNum.HasValue && request.PageNum.Value > 0)
                gallery.Skip(((int)request.PageNum - 1) * (int)request.PageSize).Take((int)request.PageSize);


            var finalResponse = new ProducerDesignsResponse { AnonName = request.UserType.ToLower() == UserType.Producer.ToString().ToLower() ? null : producer.AnonName };
            var galleryResponse = new List<GalleryResponse>();
            foreach (var item in gallery)
            {
                galleryResponse.Add(new GalleryResponse
                {
                    Design = item.Images
                    .GroupBy(img => (item.ID, img.Name ?? string.Empty))
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(img => img.ImageUrl).ToList()
                    ),
                    Notes = item.Notes,
                    Location = item.Location,
                    Category = item.Category,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                });
            }
            finalResponse.GalleryResponse = galleryResponse;
            return finalResponse;
        }
    }
}
