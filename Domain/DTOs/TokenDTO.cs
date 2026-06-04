using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class TokenDTO
    {
        public string UserID { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
