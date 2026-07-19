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
    public class GetAllGalleryQueryHandler : IRequestHandler<GetAllGalleryQuery, Result<Dictionary<string, List<string>>>>
    {
        private readonly D2DContext _context;

        public GetAllGalleryQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public async Task<Result<Dictionary<string,List<string>>>> Handle(GetAllGalleryQuery request, CancellationToken cancellationToken)
        {
            var producer = await _context.Producers.Select(p => new { p.Id, p.IsDeleted,
                Gallery = p.ProducerDesigns.Where(g=>!g.IsDeleted).Select(g=> new {g.Name, Images = g.DesignImages.Select(di=>di.ImageUrl).ToList()}) })
                .FirstOrDefaultAsync(p => p.Id == request.ProducerId&&!p.IsDeleted );

            if (producer == null)
                return Result<Dictionary<string, List<string>>>.Failure(Messages.NotFound.WithTarget("User"));

            var g = producer.Gallery;
            
            var response = producer.Gallery.GroupBy(g => g.Name)
                .ToDictionary(
                    g => g.Key ?? string.Empty,
                    g => g.SelectMany(x => x.Images).ToList()
                );
            return response;
        }
    }
}
