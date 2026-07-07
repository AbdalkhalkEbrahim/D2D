using Domain.Entities.Customers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using Domain.Enums.Types;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designers
{
    public class Report:Audits
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public UserType Reporter { get; set; }
        public Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public string ProducerID { get; set; }
        public Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public string? DesignerID { get; set; }
        public Customer Customer {  get; set; } 
        [ForeignKey("Customer")]
        public string? CustomerID { get; set; }


    }
}
