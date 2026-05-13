using Domain.Entities.Designs;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public class CustomerOffers: Offer
    {
        public int ID { get; set; }

        public OfferStatus CustomerOfferStatus { get; set; }
        public required virtual Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public required string CustomerID { get; set; }
        public required virtual CustomerDesign CustomerDesign { get; set; }
        [ForeignKey("CustomerDesign")]
        required public string CustomerDesignID { get; set; }

        public virtual ICollection<ProducerOffer> ProducerOffers { get; set; }
        public CustomerOffers()
        {
            ProducerOffers = new List<ProducerOffer>();
        }


    }
}
