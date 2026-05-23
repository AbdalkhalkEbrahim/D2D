using Domain.Entities.Designs;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public abstract class CustomerOffer: Offer
    {
        public bool Discriminator { get; set; } 
        public OfferStatus CustomerOfferStatus { get; set; }
        public required virtual Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public required string CustomerID { get; set; }
/*        public virtual ICollection<ProducerCustomerOffer> ProducerCustomerOffer { get; set; }
*/        /*public CustomerOffer()
        {
            ProducerCustomerOffer = new List<ProducerCustomerOffer>();
        }*/


    }
}
