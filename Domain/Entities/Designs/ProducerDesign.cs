using Domain.Entities.Offers;
using Domain.Entities.Producers;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designs
{
    public class ProducerDesign: Design
    {
        public virtual CustomerCustomOffer? CustomerCustomOffer { get; set; }
        [ForeignKey(nameof(CustomerCustomOffer))]
        public Guid? CustomerCustomOfferID { get; set; }
        public virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public string ProducerID { get; set; }
        public ProducerDesign():base()
        {
            Status = DesignStatus.Published;
        }
    }
}
