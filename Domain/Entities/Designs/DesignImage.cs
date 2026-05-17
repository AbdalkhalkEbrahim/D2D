using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Designs
{
    public class DesignImage
    {
        public int ID { get; set; }
        public required string ImageUrl { get; set; }
        public required virtual Design Design { get; set; }
        [ForeignKey("Design")]
        public int DesignID { get; set; }


    }
}
