using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities.Shared
{
    public abstract class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BD { get; private set; }
        public bool IsAllowed => DateTime.Now.Year - BD.Year >= 18;
        public UserType UserType { get; set; }
        public string? AnnonName { get; set; }
        public int ReportsCounter { get; set; } 
        #region IdentityVerification
        public string? FrontImageID { get; set; }
        public string? BackImageID { get; set; }
        public string? PersonalImage { get; set; }
        public VerificationStatus IdentityStatus { get; set; } = VerificationStatus.Pending;
        #endregion
        public virtual ICollection<Notification>? Notifications { get; set; }


    }
}
