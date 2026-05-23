
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.Shared
{
    public class Otp
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsUsed { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public User User { get; set; }

    }
}
