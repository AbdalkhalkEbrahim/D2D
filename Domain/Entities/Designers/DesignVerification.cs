using Domain.Enums;
using Domain.Enums.Status;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designers
{
    public class DesignVerification
    {
        [Key]
        public string StepUrl { get; set; }
        public VerificationStatus Status { get; set; }
        public virtual Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public string DesignerID { get; set; }
    }
}
