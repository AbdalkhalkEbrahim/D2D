using Application.Response;
using Domain.DTOs.Admin;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetAllProducerOffersQuery:IRequest<Result<CollaborationsWithCounts>>
    {
        public bool IsCompleted { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsCLosed { get; set; }
        public bool IsPending { get; set; }
        public bool Newest { get; set; }
        public string? OfferName { get; set; }
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 6;
    }
}
