using Domain.Entities.Designers;
using Domain.Entities.Offers;
using Domain.Entities.Producers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designs
{
    public class DesignerDesign:Design
    {
        public string ? Description { get; set; }
        public decimal? Price { get; set; }
        public virtual Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public string DesignerID { get; set; }
        public virtual ICollection<Producer> FavoritedBy { get; set; }
        public ICollection<ProducerDesignerOffer> ProducerDesignerOffers { get; set; }
        public DesignerDesign():base()
        {
            FavoritedBy = new List<Producer>();
            ProducerDesignerOffers = new List<ProducerDesignerOffer>();
        }
    }
}
