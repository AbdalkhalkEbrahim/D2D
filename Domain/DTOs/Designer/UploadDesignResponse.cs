using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Designer
{
    public class UploadDesignResponse
    {
        public int DesignId { get; set; }
        public VerificationStatus Status { get; set; }
    }
}
