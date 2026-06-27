using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Review_RateDtos
{
    public class RateResponse
    {
        public string ProducerId { get; set; }
        public decimal Rate { get; set; }
    }
}
