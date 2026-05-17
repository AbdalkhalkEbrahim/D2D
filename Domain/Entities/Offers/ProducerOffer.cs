using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public required virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public required string ProducerID { get; set; }
    }
}
