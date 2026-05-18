using Domain.Entities.Producers;
using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Designers
{
    public class Report:Audits
    {

        public required Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public required Guid ProducerID { get; set; }
        public required Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public required Guid DesignerID { get; set; }
        public required string Content { get; set; }


    }
}
