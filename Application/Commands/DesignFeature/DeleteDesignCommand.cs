using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.DesignFeature
{
    public class DeleteDesignCommand:IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
