using Domain.Entities.Customers;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Producers
{
    public class LicenseVerification
    {
        public int ID { get; set; }
        public   string LicenseUrl { get; set; }
        public VerificationStatus LicenseStatus { get; set; }
        public   virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public   Guid ProducerID { get; set; }
    }
}
