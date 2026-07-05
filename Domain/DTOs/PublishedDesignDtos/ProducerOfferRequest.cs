using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.PublishedDesignDtos
{
    public class ProducerOfferRequest
    {
        public decimal Price { get; set; }
        public decimal Diposit { get; set; }
        public int DeliveryTime { get; set; }
        public Dictionary<string, Tuple<int, int>> Steps { get; set; }
    }
}
