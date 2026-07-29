using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Producers
{
    public class ProducerGallery
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public Producer Producer { get; set; }
        [ForeignKey(nameof(Producer))]
        public string ProducerId { get; set; }
    }
}
