using Domain.Entities.Offers;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Payment
{
    public class Escrow
    {
        public Guid ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime HeldAt { get; set; }
        public DateTime ReleasedAt { get; set; }
        public EscrowStatus EscrowStatus { get; set; }
        public virtual required Transaction Transaction { get; set; }
        [ForeignKey("Transaction")]
        public required Guid TransactionID { get; set; }
        public virtual ProducerOffer? ProducerOffer { get; set; } 
        [ForeignKey("ProducerOffer")]
        public int? ProducerOfferID { get; set; }

        public int CustomerOfferID { get; set; }
        public bool IsOfferActive { get; set; }//for customer
    }
}
