using Domain.Entities.Designs;
using Domain.Enums;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public class CustomerPublishedOffer: CustomerOffer
    {
        public virtual CustomerDesign CustomerDesign { get; set; }
        [ForeignKey("CustomerDesign")]
        public Guid CustomerDesignID { get; set; }
        public virtual ICollection<ProducerCustomerOffer>? ProducerCustomerOffers { get; set; } //request from producer to customer(published)

        public CustomerPublishedOffer()
        {
            ProducerCustomerOffers = new List<ProducerCustomerOffer>();

        }


    }
}
