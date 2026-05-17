using Domain.Entities.Chats;
using Domain.Entities.Designers;
using Domain.Entities.Designs;
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
        public virtual ICollection<Review> Reviews { get;  set; }
        public virtual ICollection<ProducerDesign> ProducerDesigns { get; set; }
        public virtual ICollection<Report> Reports { get; private set; }
        public virtual ICollection<DesignerDesign> Favourite { get; set; }
        public Producer():base()
        {
            LicenseVerifications = new List<LicenseVerification>();
            ProducerOffers = new List<ProducerOffer>();
            Reviews = new List<Review>();
            Chats = new List<Chat>();
            ProducerDesigns = new List<ProducerDesign>();
            Reports = new List<Report>();
            Favourite = new List<DesignerDesign>();
        }

    }
}
