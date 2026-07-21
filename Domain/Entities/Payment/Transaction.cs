using Domain.Entities.Shared;
using Domain.Enums.Types;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;


namespace Domain.Entities.Payment
{
    public class Transaction:Audits
    {
        public Guid ID { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public   string Currency { get; set; }
        public virtual Escrow? Escrow { get; set; }
        [ForeignKey("Escrow")]
        public Guid? EscrowID { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
    }
}
