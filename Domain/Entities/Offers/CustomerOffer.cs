using Domain.Entities.Designs;
using Domain.Entities.Shared;
using Domain.Enums;
using Domain.Entities.Customers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public class CustomerOffer:Audits
    {
        public Guid ID { get; set; }
        public required string Material { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
        public bool IsActive { get; set; }
        public CustomerOfferDiscrimenator Discrimerator { get; set; } 
        public OfferStatus CustomerOfferStatus { get; set; }
        public required virtual Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public required string CustomerID { get; set; }

        public virtual ICollection<ProducerCustomerOffer> ProducerCustomerOffers { get; set; }
        public virtual ProducerCustomerOffer? ProducerCustomerOffer { get; set; } //offer from producer to customer (custome)
        [ForeignKey(nameof(ProducerCustomerOffer))]
        public Guid? ProducerCustomerOfferID { get; set; }
        /*public CustomerOffer()
        {
            ProducerCustomerOffer = new List<ProducerCustomerOffer>();
        }*/


    }
}
