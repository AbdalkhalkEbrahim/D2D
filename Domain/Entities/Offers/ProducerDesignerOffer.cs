using Domain.Entities.Designs;

namespace Domain.Entities.Offers
{
    public class ProducerDesignerOffer:ProducerOffer
    {
        public virtual DesignerDesign DesignerDesign { get; set; }
        public Guid DesignerDesignID { get; set; }

    }
}
