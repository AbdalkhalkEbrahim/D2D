using Domain.Enums.Status;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designers
{
    public class DesignVerification
    {
        public int Id { get; set; }
        public int DesignId { get; set; }
        public string FinalDesign {  get; set; }
        public List<string> StepUrl { get; set; }
        public VerificationStatus Status { get; set; }
        public virtual Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public string DesignerID { get; set; }
    }
}
