using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class SystemCounter
    {
        public int Id { get; set; } 
        public int CustomerCounter { get; set; }
        public int ProducerCounter { get; set; }
        public int DesignerCounter { get; set; }
    }
}
