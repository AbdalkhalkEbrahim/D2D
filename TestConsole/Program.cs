using Domain.Entities.Shared;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace TestConsole
{

    public class TestUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BD { get; set; }
        public bool IsAllowed => DateTime.Now.Year - BD.Year >= 18;
        public UserType UserType { get; set; }
        public string? AnonName => AnonymousName(UserType);
        public static int CCounter { get; set; }
        public static int PCounter { get; set; }
        public static int DCounter { get; set; }
        public int ReportsCounter { get; set; }
        public DateTimeOffset? OtpLockoutEnd { get; set; }
        #region IdentityVerification
        public string? FrontImageID { get; set; }
        public string? BackImageID { get; set; }
        public string? PersonalImage { get; set; }
        public VerificationStatus IdentityStatus { get; set; } = VerificationStatus.Pending;
        #endregion
        public virtual ICollection<Notification>? Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();


        private string AnonymousName(UserType userType)
        {
            int AnonCounter = (userType) switch
            {
                UserType.Customer => ++CCounter,
                UserType.Producer => ++PCounter,
                UserType.Designer => ++DCounter,
            };
            string leadingZeros = new string('0', 6 - AnonCounter.ToString().Length);
            return $"Anon{leadingZeros}{AnonCounter}_{UserType.ToString()}";
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {

            TestUser user1 = new TestUser() { UserType = UserType.Customer };
            TestUser user2 = new TestUser() { UserType = UserType.Designer };
            TestUser user4 = new TestUser() { UserType = UserType.Customer };
            TestUser user5 = new TestUser() { UserType = UserType.Designer };
            TestUser user6 = new TestUser() { UserType = UserType.Customer };

            TestUser user3 = new TestUser() { UserType = UserType.Producer };
            Console.WriteLine(user1.AnonName);
            Console.WriteLine(user2.AnonName);
            Console.WriteLine(user3.AnonName);
            Console.WriteLine(user4.AnonName);
            Console.WriteLine(user5.AnonName);
            Console.WriteLine(user6.AnonName);

        }
    }

}
