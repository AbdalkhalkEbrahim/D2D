using Domain.Enums.Status;
using Domain.Enums.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Admin
{
    public class RecentTickets
    {
        public int Id { get; set; }
        public string IssueType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
