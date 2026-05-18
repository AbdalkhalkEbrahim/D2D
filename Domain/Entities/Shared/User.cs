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
        public   string FirstName { get; set; }
        public   string LastName { get; set; }
        public DateTime BD { get; private set; }
        public bool IsAllowed { get; }
        public   string AnnonName { get; set; }
        public int ReportsCounter { get; set; } 
        #region IdentityVerification
        public   string FrontImageID { get; set; }
        public   string BackImageID { get; set; }
        public   string PersonalImage { get; set; }
        public VerificationStatus IdentityStatus { get; set; } = VerificationStatus.Pending;
        #endregion
        #region CreditInformation
        public   string CreditHoledrName { get; set; }
        public   string CreditNumber{ get; set;}
        public   string CreditExpirationDate { get; set; }
        #endregion
        public virtual ICollection<Notification>? Notifications { get; set; }

        public User()
        {
            IsAllowed = DateTime.Now.Year - BD.Year >= 18;
            Notifications = new List<Notification>();
        }

    }
}
