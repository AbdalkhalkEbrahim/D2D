using Application.Response;
using Domain.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetReadOnlyChatQuery:IRequest<Result<ReadOnlyChatResponse>>
    {
        public int ChatId { get; set; }
    }
}
