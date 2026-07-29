using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using MediatR;

namespace Application.Queries.AccountSettings
{
    public class GetAllProducerDesignsQuery:IRequest<Result<ProducerDesignsResponse>>
    {
        public string ProducerId { get; set; }
        public string UserType { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public bool Newest { get; set; } = false;
        public int? PageSize { get; set; }
        public int? PageNum { get; set; }
    }
}
