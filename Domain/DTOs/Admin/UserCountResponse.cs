using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Admin
{
    public class UserCountResponse
    {
        public int CustomerCount { get; set; }
        public int ProducerCount { get; set; }
        public double CustomerPercentage { get; set; }
        public double ProducerPercentage { get; set; }
    }
}
