using Domain.Entities.Designers;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Designs
{
    public class Design
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public DesignStatus Status { get; set; }
        public required virtual Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public required string DesignerID { get; set; }

        public virtual ICollection<DesignImage> DesignImages { get; set; }
        public Design()
        {
            DesignImages = new List<DesignImage>();
        }
    }
}
