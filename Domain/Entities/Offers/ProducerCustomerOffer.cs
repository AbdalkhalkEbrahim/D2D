using Domain.Entities.Producers;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Offers
{
    public class ProducerCustomerOffer : ProducerOffer
    {
        public virtual Producer Producer { get; set; }
        [ForeignKey(nameof(Producer))]
        public string ProducerID { get; set; }
        public virtual CustomerCustomOffer CustomerCustomOffer { get; set; }
        [ForeignKey(nameof(CustomerCustomOffer))]
        public Guid? CustomOfferId { get; set; }
        public virtual CustomerPublishedOffer? CustomerPublishedOffer { get; set; }
        [ForeignKey(nameof(CustomerPublishedOffer))]
        public Guid? CustomerPublishedOfferID { get; set; }
        public int Duration { get; set; }
        public decimal Diposit { get; set; }
        public List<ProducerSteps> Steps { get; set; }

        
    }
}
