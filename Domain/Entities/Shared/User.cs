using Domain.Enums.Status;
using Domain.Enums.Types;
using Microsoft.AspNetCore.Identity;
namespace Domain.Entities.Shared
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BD { get;  set; }
        public string? ProfileImageUrl { get; set; }
        public decimal Balance { get; set; } = 0;
        public bool IsAllowed => DateTime.Now.Year - BD.Year >= 18;
        public UserType UserType { get; set; }
        public string AnonName { get; set; } 
        public int ReportsCounter { get; set; }
        public DateTimeOffset? OtpLockoutEnd { get; set; }
        public int? OtpLockoutCount { get; set; }
        #region IdentityVerification
        public string? FrontImageID { get; set; }
        public string? BackImageID { get; set; }
        public string? PersonalImage { get; set; }
        public MagicToken? MagicToken { get; set; }
        public VerificationStatus IdentityStatus { get; set; } = VerificationStatus.Pending;
        #endregion
        public virtual ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();


      
    }
}
