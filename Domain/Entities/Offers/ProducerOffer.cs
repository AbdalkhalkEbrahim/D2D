using Domain.Entities.Shared;
using Domain.Enums;

namespace Domain.Entities.Offers
{
    public class ProducerOffer : Audits
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public OfferStatus OfferStatus { get; set; }
    }
}
