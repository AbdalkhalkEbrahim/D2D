using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class MagicToken
    {
        [Key]
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsUsed { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
    }
}
