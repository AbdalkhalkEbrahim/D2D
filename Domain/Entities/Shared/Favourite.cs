using Domain.Entities.Designs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class Favourite
    {
        public int Id { get; set; }
        public ProducerDesign ProducerDesign {  get; set; }
        [ForeignKey(nameof(Design))]
        public Guid? ProducerDesignId { get; set; }

        public DesignerDesign DesignerDesign { get; set; }
        [ForeignKey(nameof(Design))]
        public Guid? DesignerDesignId { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
