using Domain.Entities.Customers;
using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Producers
{
    public class Review:Audits
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(Customer))]
        public Guid CustomerID { get; set; }
        public   virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public Guid ProducerID { get; set; }
    }
}
