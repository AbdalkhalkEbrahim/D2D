using Domain.Entities.Shared;
using Domain.Enums.Status;

namespace Domain.Entities.Offers
{
    public abstract class ProducerOffer:Audits
    {
        public Guid ID { get; set; }
        public decimal Price { get; set; }
        public OfferStatus OfferStatus { get; set; }
/*        public virtual Producer Producer { get; set; }
        public string ProducerID { get; set; }*/
    }
}
