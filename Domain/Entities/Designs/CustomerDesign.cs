using Domain.Entities.Offers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Designs
{
    public class CustomerDesign
    {
        //multiple designs in the same design images table from different users types
        public Guid ID { get; set; }
        public required string Name { get; set; }
        public virtual ICollection<DesignImage> DesignImages { get; set; }
        public virtual ICollection<CustomerOffers> CustomerOffers { get; set; }
        public CustomerDesign()
        {
            DesignImages = new List<DesignImage>();
            CustomerOffers = new List<CustomerOffers>();
        }
    }
}
