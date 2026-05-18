using Domain.Entities.Designers;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Designs
{
    public class Design
    {
        public int ID { get; set; }
        public   string Name { get; set; }
        public DesignStatus Status { get; set; }
        public   virtual Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public   Guid DesignerID { get; set; }

        public virtual ICollection<DesignImage> DesignImages { get; set; }
        public Design()
        {
            DesignImages = new List<DesignImage>();
        }
    }
}
