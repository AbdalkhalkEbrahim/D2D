using Domain.Entities.Customers;
using Domain.Entities.Designers;
using Domain.Entities.Producers;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

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
