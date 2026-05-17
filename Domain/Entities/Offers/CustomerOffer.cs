using Domain.Entities.Customers;
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
        public required string Material { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
        public OfferStatus OfferStatus { get; set; }
        public required virtual Customer Customer { get; set; }
        [ForeignKey("Customer")]
        public required string CustomerID { get; set; }

    }
}
