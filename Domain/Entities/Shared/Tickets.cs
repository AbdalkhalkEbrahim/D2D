using Domain.Enums.Status;
using Domain.Enums.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class Tickets:Audits
    {
        public int Id { get; set; }
        public IssueType IssueType { get; set; }
        public TicketStatus Status { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
