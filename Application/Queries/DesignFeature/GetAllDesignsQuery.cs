using Application.Response;
using Domain.DTOs.DesignDtos;
using Domain.Enums.Status;
using MediatR;

namespace Application.Queries.DesignFeature
{
    public class GetAllDesignsQuery:IRequest<Result<List<DesignResponse>>>
    {
        public string CustomerId { get; set; }
        public string? Name { get; set; }
        public DateTime? Date {  get; set; }
        public DesignStatus? Status { get; set; }
    }
}
