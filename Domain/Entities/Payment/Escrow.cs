using Domain.Entities.Offers;
using Domain.Enums;
using Domain.Enums.Status;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Payment
{
    public class Escrow
    {
        public Guid ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime HeldAt { get; set; }
        public DateTime ReleasedAt { get; set; }
        public EscrowStatus EscrowStatus { get; set; }
        public virtual ICollection< Transaction> Transactions { get; set; } = new List< Transaction >();
        [ForeignKey("Transaction")]
        public Guid TransactionID { get; set; }
        public virtual ProducerOffer? ProducerOffer { get; set; } 
        [ForeignKey("ProducerOffer")]
        public Guid? ProducerOfferID { get; set; }

        public Guid CustomerOfferID { get; set; }
        public bool IsOfferActive { get; set; }//for customer
    }
}
