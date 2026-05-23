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
        public Guid ID { get; set; }
        public decimal Price { get; set; }
        public ProducerOfferDiscrimenator Discrimenator { get; set; }
        public OfferStatus OfferStatus { get; set; }
        public virtual Producer Producer { get; set; }
        public string ProducerID { get; set; }
    }
}
