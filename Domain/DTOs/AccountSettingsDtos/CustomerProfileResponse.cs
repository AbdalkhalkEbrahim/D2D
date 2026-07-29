using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AccountSettingsDtos
{
    public class CustomerProfileResponse
    {
        public ProfileResponse Profile { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Goverate { get; set; }
    }
}
