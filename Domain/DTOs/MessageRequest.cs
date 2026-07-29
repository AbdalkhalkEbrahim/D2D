using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class MessageRequest
    {
        public IFormFile? Image { get; set; }
        public string Text { get; set; }
    }
}
