using Domain.Entities.Customers;
using Domain.Entities.Payment;
using Domain.Entities.Shared;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Offers
{
    public class CustomerOffer:Audits
    {
        public int ID { get; set; }
        public   string Material { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
        public bool IsActive { get; set; } 
        public virtual ICollection<ActiveOfferLogs>? ActiveOfferLogs { get; set; }
       
        public   virtual Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public   Guid CustomerID { get; set; }
        
        public virtual Escrow? Escrow { get; set; }
        [ForeignKey("Escrow")]
        public Guid EscrowID { get; set; }
        public CustomerOffer()
        {
            ActiveOfferLogs = new List<ActiveOfferLogs>();
        }

    }
}
