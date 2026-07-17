using Application.Queries.OffersFeature;
using Application.Response;
using Domain.DTOs.OfferDtos;
using Domain.Enums.Status;
using Infrastructure.Data.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Handlers.OffersFeature
{
    public class GetAllPublishedDesignsQueryHandler : IRequestHandler<GetAllPublishedesignsQuery, Result<List<CustomerOfferResponse>>>
    {
        private readonly D2DContext _context;

        public GetAllPublishedDesignsQueryHandler(D2DContext context)
        {
            _context = context;    
        }
        public async Task<Result<List<CustomerOfferResponse>>> Handle(GetAllPublishedesignsQuery request, CancellationToken cancellationToken)
        {
            var design = _context.CustomerDesigns/*.Include(cd => cd.CustomerPublishedOffer).Include(cd=>cd.DesignImages)
                .Include(cd=>cd.CustomerPublishedOffer.ProducerCustomerOffers)*/
                .Select(cd=>new
                {
                    cd.CustomerPublishedOffer.ID,
                    cd.CustomerId,
                    cd.CustomerPublishedOffer.Name,
                    cd.CustomerPublishedOffer.Description,
                    cd.CustomerPublishedOffer.Duration,
                    cd.CustomerPublishedOffer.MaxPrice,
                    cd.CustomerPublishedOffer.Amount,
                    cd.CustomerPublishedOffer.Category,
                    cd.CustomerPublishedOffer.Gender,
                    cd.CustomerPublishedOffer.CreatedAt,
                    DesignImages = cd.DesignImages.Select(di => di.ImageUrl),
                    cd.CustomerPublishedOffer.Colors,
                    cd.CustomerPublishedOffer.Material,
                    cd.CustomerPublishedOffer.PrintingType,
                    cd.CustomerPublishedOffer.Sizes,
                    cd.CustomerPublishedOffer.SizesFile,
                    cd.Status,
                    cd.CustomerPublishedOffer.UpdatedAt,
                    cd.CustomerPublishedOffer.IsActive,
                    cd.CustomerPublishedOffer.TargetAudience,
                    IDs = cd.CustomerPublishedOffer.ProducerCustomerOffers.Select(pco => pco.ID),
                    cd.Customer.Addresses.FirstOrDefault(add=>add.Selected).City

                })
                .AsNoTracking()
                .Where(d => d.Status == DesignStatus.Published&& !d.IsActive );

            //Console.WriteLine(design is null);

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
           var designs =  design.ToList();
            foreach (var item in designs)
            {
                result.Add
                    (
                         new CustomerOfferResponse
                         {
                             PublishedOfferID =(Guid) item.ID,
                             CustomerId = item.CustomerId,
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
                             ProducersOffersIDs = item.IDs.ToList()
                         }
                    );
            }
            return result;
        }
    }
}
