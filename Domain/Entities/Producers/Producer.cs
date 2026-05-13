using Domain.Entities.Offers;
using Domain.Entities.Shared;

namespace Domain.Entities.Producer
{
    public class Producer:User
    {
        public double Rate { get; set; }
        public virtual ICollection<LicenseVerification> LicenseVerifications { get; set; }
        public virtual ICollection<ProducerOffer> ProducerOffers { get; set; }
        public Producer()
        {
            LicenseVerifications = new List<LicenseVerification>();
            ProducerOffers = new List<ProducerOffer>();
        }

    }
}
