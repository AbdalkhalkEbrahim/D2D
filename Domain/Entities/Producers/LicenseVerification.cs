using Domain.Enums;
using Domain.Enums.Status;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Producers
{
    public class LicenseVerification
    {
        public int ID { get; set; }
        public   string LicenseUrl { get; set; }
        public VerificationStatus LicenseStatus { get; set; }
        public   virtual Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public string ProducerID { get; set; }
    }
}
