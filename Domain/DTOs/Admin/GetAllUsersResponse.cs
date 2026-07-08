using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Admin
{
    public class GetAllUsersResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int NumOfReports { get; set; }
        public int NumOfCollations { get; set; }
        public DateTime JoinDate { get; set; }
    }
}
