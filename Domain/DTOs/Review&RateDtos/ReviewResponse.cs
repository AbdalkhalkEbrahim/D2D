using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Review_RateDtos
{
    public class ReviewResponse
    {
        public int ID { get; set; }
        
        public string Content { get; set; }
        public double Rate { get; set; }
        public string ProducerAnonName { get; set; }
        public string CustomerAnonName { get; set; }
        public string ProducerId { get; set; }
        public double ProducerRate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
