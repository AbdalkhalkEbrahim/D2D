using Domain.Entities.Designers;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Designs
{
    public class Design
    {
        public Guid ID { get; set; }
        public   string Name { get; set; }
        public DesignStatus Status { get; set; }
        public virtual ICollection<DesignImage> DesignImages { get; set; }
        public Design()
        {
            DesignImages = new List<DesignImage>();
        }
    }
}
