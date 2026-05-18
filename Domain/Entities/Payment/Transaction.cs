using Domain.Entities.Offers;
using Domain.Entities.Shared;
using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.Payment
{
    public class Transaction:Audits
    {
        public Guid ID { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public required string Currency { get; set; }
        public virtual required ICollection<Escrow> Escrows { get; set; }
        public virtual required User User { get; set; }

        [ForeignKey("User")]
        public Guid UserID { get; set; }


        public Transaction()
        {
            Escrows = new HashSet<Escrow>();
        }

    }
}
