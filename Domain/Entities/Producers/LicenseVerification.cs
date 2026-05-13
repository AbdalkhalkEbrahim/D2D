using Domain.Entities.Customers;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Producer
{
    public class LicenseVerification
    {
        public int ID { get; set; }
        public required string LicenseUrl { get; set; }
        public UserVerificationStatus LicenseStatus { get; set; }
    }
}
