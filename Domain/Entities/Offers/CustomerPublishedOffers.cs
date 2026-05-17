using Domain.Entities.Designs;
using Domain.Enums;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public class CustomerPublishedOffers: CustomerOffer
    {

        public required virtual CustomerDesign CustomerDesign { get; set; }
        [ForeignKey("CustomerDesign")]
         public required string CustomerDesignID { get; set; }
        public virtual ICollection<ProducerCustomerOffer>? ProducerOffers { get; set; } //request from producer to customer(published)

        public CustomerPublishedOffers()
        {
            ProducerOffers = new List<ProducerCustomerOffer>();

        }


    }
}
