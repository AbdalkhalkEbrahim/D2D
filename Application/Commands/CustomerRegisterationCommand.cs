using Domain.Entities.Customers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class CustomerRegisterationCommand
    {
        public string CustomerId { get; set; }
        public IFormFile FrontImageID { get; set; }
        public IFormFile BackImageID { get; set; }
        public IFormFile PersonalImage { get; set; } 
        public string AppartmentNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Goverate { get; set; }
    }
}
