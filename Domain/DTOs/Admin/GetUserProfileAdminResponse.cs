using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Admin
{
    public class GetUserProfileAdminResponse
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string? AddressForCustomer { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string AnonName { get; set; }
        public DateTime JoinDate { get; set; }
        public int ReportCount { get; set; }
        public int TotalCollaborations { get; set; }
        public int TotalEarned { get; set; }
        public string FrontSideIdUrl { get; set; }
        public string BackSideIdUrl { get; set; }
        public string PersonalImageUrl { get; set; }
        public List<string>? LicenseUrlForProducer { get; set; }
    }
}
