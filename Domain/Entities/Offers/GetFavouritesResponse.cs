using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Offers
{
    public class GetFavouritesResponse
    {
        public string ProducerAnnon { get; set; }
        public string ProducerId { get; set; }
        public Guid DesignId { get; set; }
        public List<string> DesignImages { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
