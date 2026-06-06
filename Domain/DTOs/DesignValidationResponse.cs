using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class DesignValidationResponse
    {
        public bool OwnershipVerified { get; set; }
        public float? ConfidenceScore { get; set; }
        public string DetectedRisks { get; set; }
        public int? ProgressScore { get; set; }
        public string DetailedExplanation { get; set; }

    }
}
