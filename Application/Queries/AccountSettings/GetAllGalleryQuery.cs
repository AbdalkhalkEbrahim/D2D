using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using MediatR;

namespace Application.Queries.AccountSettings
{
    public class GetAllGalleryQuery:IRequest<Result<List<GalleryResponse>>>
    {
        public string ProducerId { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }
        public string? Name { get; set; }
        public int? PageSize { get; set; }
        public int? PageCount { get; set; }
    }
}
