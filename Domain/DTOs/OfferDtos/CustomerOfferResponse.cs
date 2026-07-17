using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.OfferDtos
{
    public class CustomerOfferResponse
    {
        public Guid PublishedOfferID { get; set; }
        public List<string> DesignImages { get; set; }
        public string? CustomerId { get; set; } 
        public string Name { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string TargetAudience { get; set; }
        public string Gender { get; set; }
        public List<string> Colors { get; set; }
        public string? Material { get; set; }
        public string? PrintingType { get; set; }
        public List<string> Sizes { get; set; }

        public string? SizesFile { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
        public bool IsActive { get; set; }
        public List<Guid>? ProducersOffersIDs { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
