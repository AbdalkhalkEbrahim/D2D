using Domain.Entities.Customers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AccountSettingsDtos
{
    public class ProfileResponse
    {
        public  string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string AnonName { get; set; }
        public DateTime BD { get; set; }
       
    }
}
