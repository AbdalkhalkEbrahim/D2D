using Domain.Enums.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.DesignDtos
{
    public class DesignResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public List<string> Images { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PublishedAt { get; set; } = null;

    }
}
