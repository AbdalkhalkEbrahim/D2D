using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Review_RateDtos
{
    public class ReviewAndRateResponse
    {
        public double ProducerRate { get; set; }
        public int TotalRates { get; set; }
        public List<ReviewResponse> Reviews { get; set; }

    }
}
