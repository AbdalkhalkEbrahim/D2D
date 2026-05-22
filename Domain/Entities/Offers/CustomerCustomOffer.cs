using Domain.Entities.Designs;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public class CustomerCustomOffer:CustomerOffer
    {
        public required virtual ProducerDesign ProducerDesign { get; set; }
        [ForeignKey("ProducerDesign")]
        public Guid ProducerDesignID { get; set; }
        public virtual ProducerCustomerOffer? ProducerCustomerOffer { get; set; } //offer from producer to customer (custome)
        [ForeignKey(nameof(ProducerCustomerOffer))]
        public Guid? ProducerCustomerOfferID { get; set; }
/*        public CustomerCustomOffer():base(){}
*/    }
}
