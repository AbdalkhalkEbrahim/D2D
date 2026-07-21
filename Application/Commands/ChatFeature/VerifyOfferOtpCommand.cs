using Application.Response;
using MediatR;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ChatFeature
{
    public class VerifyOfferOtpCommand:IRequest<Result<string>>
    {
        public string ProducerId { get; set; }
        public string CustomerId { get; set; }
        public int ChatId { get; set; }
        public string Code { get; set; }
    }
}
