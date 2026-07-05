using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.PublishedDesignDtos
{
    public class PublishedDesignRequest
    {
        public string Category { get; set; }
        public string Description { get; set; }
        public string TargetAudience { get; set; }
        public bool Gender { get; set; }
        public List<string> Colors { get; set; }
        public string? Material { get; set; }
        public string? PrintingType { get; set; }
        public List<string> Sizes { get; set; }
        public IFormFile? SizesFile { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
