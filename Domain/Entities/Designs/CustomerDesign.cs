using Domain.Entities.Offers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designs
{
    public class CustomerDesign:Design
    {
        //multiple designs in the same design images table from different users types
        public virtual CustomerPublishedOffer? CustomerPublishedOffer { get; set; }
        [ForeignKey(nameof(CustomerPublishedOffer))]
        public Guid? CustomerPublishedOfferID { get; set; }
        public CustomerDesign():base()
        {
        }
    }
}
