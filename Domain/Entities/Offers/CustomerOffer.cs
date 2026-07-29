using Domain.Entities.Shared;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums.Status;

namespace Domain.Entities.Offers
{
    public abstract class CustomerOffer: Offer
    {
        public OfferStatus CustomerOfferStatus { get; set; }
        public virtual Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public string CustomerID { get; set; }
/*        public virtual ICollection<ProducerCustomerOffer> ProducerCustomerOffer { get; set; }
*/        /*public CustomerOffer()
        {
            ProducerCustomerOffer = new List<ProducerCustomerOffer>();
        }*/


    }
}
