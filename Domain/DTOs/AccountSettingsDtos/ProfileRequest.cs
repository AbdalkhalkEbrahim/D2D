using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.AccountSettingsDtos
{
    public class ProfileRequest
    {
        [RegularExpression("^[a-zA-O-Z-a-z]{1,30}$", ErrorMessage = "Invalid name format")]
        public string? FirstName { get; set; }
        [RegularExpression("^[a-zA-O-Z-a-z]{1,30}$", ErrorMessage = "Invalid name format")]
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
