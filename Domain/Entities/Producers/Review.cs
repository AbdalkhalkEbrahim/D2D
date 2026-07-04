using Domain.Entities.Customers;
using Domain.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Producers
{
    public class Review:Audits
    {
        public int ID { get; set; }
        public int Rate { get; set; }
        public string Content { get; set; }
        public virtual Customer Customer { get; set; }

        [ForeignKey(nameof(Customer))]
        public string CustomerID { get; set; }
        public   virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public string ProducerID { get; set; }
    }
}
