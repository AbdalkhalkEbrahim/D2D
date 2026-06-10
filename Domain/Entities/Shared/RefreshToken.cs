using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Shared
{
    public class RefreshToken
    {
        public int ID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }

    }
}
