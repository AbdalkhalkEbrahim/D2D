using Domain.Entities.Designs;
using Domain.Entities.Shared;

namespace Domain.Entities.Designers
{
    public class Designer:User
    {
        public virtual ICollection<DesignerDesign> DesignerDesigns { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<DesignVerification> DesignVerifications { get; set; }

        public Designer()
        {
            DesignerDesigns = new List<DesignerDesign>();
            Reports = new List<Report>();
            DesignVerifications = new List<DesignVerification>();

        }
    }
}
