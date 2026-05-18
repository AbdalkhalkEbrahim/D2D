using Domain.Entities.Designs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Offers
{
    public class CustomerCustomOffer:CustomerOffer
    {
        public   virtual ProducerDesign ProducerDesign { get; set; }
        [ForeignKey("ProducerDesign")]
        public   int ProducerDesignID { get; set; }
        public virtual ProducerCustomerOffer? ProducerCustomerOffer { get; set; } //offer from producer to customer (custome)
        [ForeignKey(nameof(ProducerCustomerOffer))]
        public int? ProducerCustomerOfferID { get; set; }

    }
}
