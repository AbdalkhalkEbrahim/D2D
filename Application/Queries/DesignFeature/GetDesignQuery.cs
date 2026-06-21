using Application.Response;
using Domain.DTOs.DesignDtos;
using MediatR;

namespace Application.Queries.DesignFeature
{
    public class GetDesignQuery:IRequest<Result<DesignResponse>>
    {
        public Guid Id { get; set; }
    }
}
