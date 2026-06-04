using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Designs
{
    public class DesignImage
    {
        public int ID { get; set; }
        public   string ImageUrl { get; set; }
        public virtual CustomerDesign? CustomerDesign { get; set; }
        public virtual ProducerDesign? ProducerDesign { get; set; }
        public virtual DesignerDesign? DesignerDesign { get; set; }
        [ForeignKey(nameof(CustomerDesign))]
        public Guid? CustomerDesignID { get; set; }
        [ForeignKey(nameof(ProducerDesign))]
        public Guid? ProducerDesignID { get; set; }
        [ForeignKey(nameof(DesignerDesign))]
        public Guid? DesignerDesignID { get; set; }
    }
}
