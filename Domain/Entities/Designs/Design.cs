using Domain.Entities.Shared;
using Domain.Enums.Status;
namespace Domain.Entities.Designs
{
    public abstract class Design:Audits
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
