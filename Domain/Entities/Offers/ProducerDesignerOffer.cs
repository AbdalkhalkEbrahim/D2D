using Domain.Entities.Designs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Offers
{
    public class ProducerDesignerOffer:ProducerOffer
    {
        public   virtual DesignerDesign DesignerDesign { get; set; }
        [ForeignKey(nameof(DesignerDesign))]
        public int DesignerDesignID { get; set; }
    }
}
