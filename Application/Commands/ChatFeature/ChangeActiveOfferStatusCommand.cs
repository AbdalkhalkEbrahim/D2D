using Application.Response;
using Domain.Enums.Status;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatFeature
{
    public class ChangeActiveOfferStatusCommand : IRequest<Result<string>>
    {
        public int ChatID { get; set; } 
        public ActiveOfferStatus Status { get; set; }
    }
}
