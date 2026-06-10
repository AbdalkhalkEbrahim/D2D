using Domain.Entities.Producers;
using Domain.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Designers
{
    public class Report:Audits
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public bool IsProducer { get; set; }
        public Producer Producer { get; set; }
        [ForeignKey("Producer")]
        public string ProducerID { get; set; }
        public Designer Designer { get; set; }
        [ForeignKey("Designer")]
        public string DesignerID { get; set; }


    }
}
