using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.DesignDtos
{
    public class DesignsSaved
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public IFormFile DesignImage { get; set; }
        public string Notes { get; set; }
    }
}
