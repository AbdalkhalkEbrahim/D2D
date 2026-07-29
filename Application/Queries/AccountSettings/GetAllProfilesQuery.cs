using Application.Response;
using Domain.DTOs.AccountSettingsDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.AccountSettings
{
    public class GetAllProfilesQuery:IRequest<Result<List<ProducersProfilesReponse>>>
    {

    }
}
