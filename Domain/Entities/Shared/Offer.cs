using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class Offer:Audits
    {
        public required string Material { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
