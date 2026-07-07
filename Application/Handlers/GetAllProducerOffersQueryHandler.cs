using Application.Queries;
using Application.Response;
using Domain.DTOs;
using Infrastructure.Data.Context;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class GetAllProducerOffersQueryHandler : IRequestHandler<GetAllProducerOffersQuery, Result<List<CollaborationResponse>>>
    {
        private readonly D2DContext _context;

        public GetAllProducerOffersQueryHandler(D2DContext context)
        {
            _context = context;
        }
        public Task<Result<List<CollaborationResponse>>> Handle(GetAllProducerOffersQuery request, CancellationToken cancellationToken)
        {
            var offer = _context.CustomerPublishedOffers.Select(c => new { c.ID, c.Name, c.CustomerDesign.DesignImages, c.Customer.FirstName, c.Customer.LastName, c.Customer.ProfileImageUrl, ProducerOffers = c.ProducerCustomerOffers.Select(p => new { p.Producer.FirstName, p.Producer.LastName, p.Producer.ProfileImageUrl, p.Price, p.Diposit, p.OfferStatus, p.CreatedAt }) });
        }
    }
}
