using Application.Queries.OffersFeature.CustomOffers;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers.OffersFeature.CustomOffers
{
    public class GetCustomersRequestsQueryHandler : IRequestHandler<GetCustomersRequestsQuery, Result<List<CustomerOfferResponse>>>
    {
        private readonly D2DContext _context;

        public GetCustomersRequestsQueryHandler(D2DContext context)
        {
            _context = context;
        }

        public async Task<Result<List<CustomerOfferResponse>>> Handle(GetCustomersRequestsQuery request, CancellationToken cancellationToken)
        {
            var design = _context.CustomerCustomOffers
                .Select(cpo => new
                {
                    cpo.ProducerDesignID,
                    cpo.ID,
                    DesignImages = cpo.ProducerDesign.DesignImages.Select(di => di.ImageUrl),
                    cpo.Name,
                    cpo.Customer.Addresses.FirstOrDefault(add => add.Selected).City,
                    cpo.Description,
                    cpo.Category,
                    cpo.Amount,
                    cpo.Duration,
                    cpo.Colors,
                    cpo.Gender,
                    cpo.Material,
                    cpo.MaxPrice,
                    cpo.PrintingType,
                    cpo.Sizes,
                    cpo.SizesFile,
                    cpo.TargetAudience,
                    cpo.IsActive,
                    cpo.CreatedAt,
                    cpo.UpdatedAt,
                    cpo.Customer.IsDeleted,
                    cpo.CustomerID,
                    cpo.CustomerOfferStatus,
                    cpo.ProducerDesign.ProducerID
                })
                .AsNoTracking()
                .Where(d =>d.ProducerID==request.ProducerId&& d.CustomerOfferStatus == OfferStatus.OnHold && !d.IsActive && !d.IsDeleted);

            if (request.DesignId != null)
                design = design.Where(d => d.ProducerDesignID == request.DesignId);

            if (request.Duration == 1)
                design = design.OrderBy(d => d.Duration);
            else if (request.Duration == 2)
                design = design.OrderByDescending(d => d.Duration);


            if (request.MaxPrice == 1)
                design = design.OrderBy(d => d.MaxPrice);
            else if (request.MaxPrice == 2)
                design = design.OrderByDescending(d => d.MaxPrice);

            if (request.Amount == 1)
                design = design.OrderBy(d => d.Amount);
            else if (request.Amount == 2)
                design = design.OrderByDescending(d => d.Amount);

            if (request.Gender != 0)
                design = design.Where(d => d.Gender == (request.Gender == 2));

            if (!string.IsNullOrEmpty(request.Category))
                design = design.Where(d => d.Category == request.Category);

            if (request.CreationOrder == 1)
                design = design.OrderBy(d => d.CreatedAt);
            else if (request.CreationOrder == 2)
                design = design.OrderByDescending(d => d.CreatedAt);

            if (await design.AnyAsync() == false)
                return Result<List<CustomerOfferResponse>>.Failure(Messages.NotFound.WithTarget("Design"));

            if (request.PageNum.HasValue && request.PageNum.Value > 0)
                design = design.Skip(((int)request.PageNum - 1) * (int)request.PageSize).Take((int)request.PageSize);

            var result = new List<CustomerOfferResponse>();
            var designs = design.ToList();
            foreach (var item in designs)
            {
                result.Add
                    (
                         new CustomerOfferResponse
                         {
                             PublishedOfferID = (Guid)item.ID,
                             CustomerId = item.CustomerID,
                             DesignImages = item.DesignImages.ToList(),
                             Name = item.Name,
                             City = item.City,
                             Category = item.Category,
                             Description = item.Description,
                             Amount = item.Amount,
                             Colors = item.Colors,
                             Duration = item.Duration,
                             Gender = !item.Gender ? "Male" : "Female",
                             Material = item.Material,
                             MaxPrice = item.MaxPrice,
                             PrintingType = item.PrintingType,
                             Sizes = item.Sizes,
                             SizesFile = item.SizesFile,
                             TargetAudience = item.TargetAudience,
                             IsActive = item.IsActive,
                             PublishedAt = item.CreatedAt,
                             UpdatedAt = item.UpdatedAt,
                         }
                    );
            }
            return result;
        }
    }
}
