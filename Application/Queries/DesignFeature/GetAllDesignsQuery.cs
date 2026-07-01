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
        public DateTime? StartDate {  get; set; }
        public DateTime? EndDate { get; set; }
        public DesignStatus? Status { get; set; }
        public int? PageSize { get; set; } = 10;
        public int? PageNum { get; set; } = 1;
    }
}
