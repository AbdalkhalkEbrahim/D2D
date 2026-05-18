using Domain.Enums;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime BD { get; private set; }
        public bool IsAllowed { get; }
        public required string AnnonName { get; set; }
        #region IdentityVerification
        public required string FrontImageID { get; set; }
        public required string BackImageID { get; set; }
        public required string PersonalImage { get; set; }
        public VerificationStatus IdentityStatus { get; set; } = VerificationStatus.Pending;
        #endregion
        #region CreditInformation
        public required string CreditHoledrName { get; set; }
        public required string CreditNumber{ get; set;}
        public required string CreditExpirationDate { get; set; }
        #endregion
        public virtual ICollection<Notification>? Notifications { get; set; }

        public User()
        {
            IsAllowed = DateTime.Now.Year - BD.Year >= 18;
            Notifications = new List<Notification>();
        }

    }
}
