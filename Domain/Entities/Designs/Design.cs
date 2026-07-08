using Domain.Entities.Chats.AiModel;
using Domain.Entities.Shared;
using Domain.Enums.Status;
using System.ComponentModel.DataAnnotations.Schema;
namespace Domain.Entities.Designs
{
    public class Design:Audits
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
