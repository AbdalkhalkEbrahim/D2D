using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Admin
{
    public class GetUserAndStatusCount
    {
        public int AllUsersCount { get; set; }
        public int AllCustomersCount { get; set; }
        public int AllProducersCount { get; set; }
        public int AllStatusesCount { get; set; }
        public int ActiveStatusCount { get; set; }
        public int PendingStatusCount { get; set; }
        public int SusbendStatusCount { get; set; }

        public List<GetAllUsersResponse> UsersResponse { get; set; }
    }
}
