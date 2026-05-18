using Domain.Entities.Shared;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification:Audits
    {
        public int ID { get; set; }
        public NotificationsType NotificationsType { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public bool IsRead { get; set; }
        public required string RefrenceUrl { get; set; }

        public virtual required User User { get; set; }
        [ForeignKey("User")]
        public required Guid UserID { get; set; }


    }
}
