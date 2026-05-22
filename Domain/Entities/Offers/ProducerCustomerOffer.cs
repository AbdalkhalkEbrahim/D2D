using Domain.Entities.Shared;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public class ProducerCustomerOffer : ProducerOffer
    {
        public virtual CustomerCustomOffer? CustomerCustomOffer { get; set; }
        [ForeignKey(nameof(CustomerCustomOffer))]
        public Guid? CustomerCustomOfferID { get; set; }
        public virtual CustomerPublishedOffer? CustomerPublishedOffer { get; set; }
        [ForeignKey(nameof(CustomerPublishedOffer))]
        public Guid CustomerPublishedOfferID { get; set; }

    }
}
