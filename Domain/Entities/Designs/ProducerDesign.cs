using Domain.Entities.Offers;
using Domain.Entities.Producers;
using Domain.Enums.Status;
using Domain.Enums.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designs
{
    public class ProducerDesign: Design
    {
        public DesignType DesignType { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection< CustomerCustomOffer> CustomerCustomOffers { get; set; }
       
        public virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public string ProducerID { get; set; }
        public ProducerDesign():base()
        {
            Status = DesignStatus.Published;
            CustomerCustomOffers=new List<CustomerCustomOffer>();
        }
    }
}
