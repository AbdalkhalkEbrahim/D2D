using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum EscrowStatus
    {
        /// <summary>
        /// The contract is created, waiting for the Customer to successfully deposit the funds.
        /// </summary>
        PendingDeposit = 1,

        /// <summary>
        /// Funds are safely cleared and securely locked in the escrow vault. The Producer can safely begin work.
        /// </summary>
        Held = 2,

        /// <summary>
        /// The Producer submitted the work, and the system is waiting for the Customer's explicit approval or a timeout.
        /// </summary>
        AwaitingApproval = 3,

        /// <summary>
        /// The Customer raised a formal complaint. Funds remain locked until platform admins arbitrate.
        /// </summary>
        Disputed = 4,

        /// <summary>
        /// Conditions met or dispute resolved in favor of the Producer. Funds are unlocked and moving to the Producer.
        /// </summary>
        Released = 5,

        /// <summary>
        /// Order cancelled or dispute resolved in favor of the Customer. Funds are unlocked and returning to the Customer.
        /// </summary>
        ReturnedToCustomer = 6
    }
}
