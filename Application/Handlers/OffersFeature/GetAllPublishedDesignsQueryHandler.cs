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
            var design = _context.CustomerDesigns.Include(cd => cd.CustomerPublishedOffer).Include(cd=>cd.DesignImages).Include(cd=>cd.CustomerPublishedOffer.ProducerCustomerOffers).AsNoTracking().Where(d => d.Status == DesignStatus.Published&&d.CustomerPublishedOffer.ProducerCustomerOffers!=null);
            Console.WriteLine(design is null);

            if (request.Duration == 1)
                design = design.OrderBy(d => d.CustomerPublishedOffer.Duration);
            else if (request.Duration == 2)
                design = design.OrderByDescending(d => d.CustomerPublishedOffer.Duration);


            if (request.MaxPrice == 1)
                design = design.OrderBy(d => d.CustomerPublishedOffer.MaxPrice);
            else if (request.MaxPrice == 2)
                design = design.OrderByDescending(d => d.CustomerPublishedOffer.MaxPrice);

            if (request.Amount == 1)
                design = design.OrderBy(d => d.CustomerPublishedOffer.Amount);
            else if (request.Amount == 2)
                design = design.OrderByDescending(d => d.CustomerPublishedOffer.Amount);

            if (request.Gender != 0)
                design = design.Where(d => d.CustomerPublishedOffer.Gender == (request.Gender == 2));

            if (!string.IsNullOrEmpty(request.Category))
                design = design.Where(d => d.CustomerPublishedOffer.Category == request.Category);

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
                             PublishedOfferID =(Guid) item.CustomerPublishedOffer.ID,
                             CustomerId = item.CustomerId,
                             DesignImages = item.DesignImages.Select(di => di.ImageUrl).ToList(),
                             Name = item.Name,
                             Category = item.CustomerPublishedOffer.Category,
                             Description = item.CustomerPublishedOffer.Description,
                             Amount = item.CustomerPublishedOffer.Amount,
                             Colors = item.CustomerPublishedOffer.Colors,
                             Duration = item.CustomerPublishedOffer.Duration,
                             Gender = !item.CustomerPublishedOffer.Gender ? "Male" : "Female",
                             Material = item.CustomerPublishedOffer.Material,
                             MaxPrice = item.CustomerPublishedOffer.MaxPrice,
                             PrintingType = item.CustomerPublishedOffer.PrintingType,
                             Sizes = item.CustomerPublishedOffer.Sizes,
                             SizesFile = item.CustomerPublishedOffer.SizesFile,
                             TargetAudience = item.CustomerPublishedOffer.TargetAudience,
                             IsActive = item.CustomerPublishedOffer.IsActive,
                             PublishedAt = item.CustomerPublishedOffer.CreatedAt,
                             UpdatedAt = item.CustomerPublishedOffer.UpdatedAt,
                             ProducersOffersIDs = item.CustomerPublishedOffer.ProducerCustomerOffers.Select(pco => pco.ID).ToList()
                         }
                    );
            }
            return result;
        }
    }
}
