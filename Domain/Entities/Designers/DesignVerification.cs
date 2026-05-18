using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Designers
{
    public class DesignVerification
    {
        [Key]
        public string StepUrl { get; set; }
        public VerificationStatus Status { get; set; }
        public virtual Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public Guid DesignerID { get; set; }
    }
}
