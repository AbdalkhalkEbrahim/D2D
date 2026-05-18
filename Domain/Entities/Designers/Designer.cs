using Domain.Entities.Designs;
using Domain.Entities.Shared;

namespace Domain.Entities.Designers
{
    public class Designer : User
    {
        public virtual ICollection<Design> Designs { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<DesignVerification> DesignVerifications { get; set; }

        public Designer() : base()
        {
            Designs = new List<Design>();
            Reports = new List<Report>();
            DesignVerifications = new List<DesignVerification>();
        }
    }
}
