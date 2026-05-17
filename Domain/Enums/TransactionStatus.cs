using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum TransactionStatus
    {
        /// <summary>
        /// The payment intent has been created in the system, but the user hasn't completed checkout.
        /// </summary>
        Initiated = 1,

        /// <summary>
        /// The customer's bank has verified and locked the funds (Hold placed), but the money hasn't been pulled yet.
        /// </summary>
        Authorized = 2,

        /// <summary>
        /// The money has been successfully debited from the customer and is now held by your platform.
        /// </summary>
        Captured = 3,

        /// <summary>
        /// The payment gateway rejected the charge (e.g., insufficient funds, 3D Secure failure).
        /// </summary>
        Failed = 4,

        /// <summary>
        /// The customer abandoned the payment or it timed out before completion.
        /// </summary>
        Cancelled = 5,

        /// <summary>
        /// The funds have been transferred completely out of the platform into the Producer's external bank account.
        /// </summary>
        Settled = 6,

        /// <summary>
        /// The transaction was completely rolled back and the funds were returned to the Customer's bank account.
        /// </summary>
        Refunded = 7
    }
}
