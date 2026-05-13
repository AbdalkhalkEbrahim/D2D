using Domain.Entities.Shared;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Offers
{
    public class ProducerOffer:Audits
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public OfferStatus OfferStatus { get; set; }
    }
}
