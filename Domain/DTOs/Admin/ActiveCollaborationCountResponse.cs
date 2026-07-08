using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Admin
{
    public class ActiveCollaborationCountResponse
    {
        public int ActiveCollaborationCount { get; set; }
        public double Percentage { get; set; }
    }
}
