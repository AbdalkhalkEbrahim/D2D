using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DesignFeature
{
    public class PublishedToDraftedCommand: IRequest<Result<Guid>>
    {
        public Guid DesignId { get; set; }
    }
}
