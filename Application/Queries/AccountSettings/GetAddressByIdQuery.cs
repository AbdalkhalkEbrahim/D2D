using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using MediatR;

namespace Application.Queries.AccountSettings
{
    public class GetAddressByIdQuery : IRequest<Result<AddressResponse>>
    {
        public int AddressId { get; set; }
    }
}
