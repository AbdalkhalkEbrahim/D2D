using Domain.Entities.Offers;
using Domain.Entities.Producers;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Designs
{
    public class ProducerDesign: Design
    {
        public virtual CustomerCustomOffer? CustomerCustomOffer { get; set; }
        [ForeignKey(nameof(CustomerCustomOffer))]
        public int? CustomerCustomOfferID { get; set; }
        public   virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public   Guid ProducerID { get; set; }
        public ProducerDesign():base()
        {
            Status = DesignStatus.Published;
        }
    }
}
