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
        public required string StepUrl { get; set; }
        public VerificationStatus Status { get; set; }
        public required virtual Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public required string DesignerID { get; set; }
    }
}
