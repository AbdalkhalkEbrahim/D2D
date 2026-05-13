using Domain.Entities.Chats;
using Domain.Entities.Offers;
using Domain.Entities.Shared;

namespace Domain.Entities.Producers
{
    public class Producer:User
    {
        public double Rate { get; set; }
        public virtual ICollection<LicenseVerification> LicenseVerifications { get; set; }
        public virtual ICollection<ProducerOffer> ProducerOffers { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public List<Review> Reviews { get;  set; }

        public Producer()
        {
            LicenseVerifications = new List<LicenseVerification>();
            ProducerOffers = new List<ProducerOffer>();
            Reviews = new List<Review>();
            Chats = new List<Chat>();
        }

    }
}
