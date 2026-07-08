using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class CollaborationsWithCounts
    {

        public int AllCount { get; set; }
        public int CompletedCount { get; set; }
        public int ClosedCount { get; set; }
        public int PendingCount { get; set; }
        public List<CollaborationResponse> collaborationResponses { get; set; }
    }
}
