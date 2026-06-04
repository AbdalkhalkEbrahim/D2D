using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class IdentityValidationRequest
    {
        public string FrontImageUrl { get; set; }
        public string BackImageUrl { get; set; }
        public string SelfieImageUrl { get; set; }
    }
}
