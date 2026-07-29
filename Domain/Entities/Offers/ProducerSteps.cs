using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Offers
{
    public class ProducerSteps
    {
        public int Id { get; set; }
        public string StepName { get; set; }
        public int MinDuration { get; set; }
        public int MaxDuration { get; set; }
        public ProducerCustomerOffer ProducerCustomerOffer { get; set; }
        [ForeignKey(nameof(ProducerCustomerOffer))]
        public Guid ProducerCustomerOfferId { get; set; }
    }
}
