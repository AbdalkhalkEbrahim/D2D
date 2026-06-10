using Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Shared
{
    public class Notification : Audits
    {
        public int ID { get; set; }
        public NotificationsType NotificationsType { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public string RefrenceUrl { get; set; }

        public virtual User User { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }


    }
}
