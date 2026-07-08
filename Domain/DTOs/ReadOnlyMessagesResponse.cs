using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ReadOnlyMessagesResponse
    {
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public List<string>Content { get; set; }
        public DateTime SentAt { get; set; }
 
    }
}
