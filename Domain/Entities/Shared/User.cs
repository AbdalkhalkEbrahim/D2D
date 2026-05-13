using Domain.Enums;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Shared
{
    public class User : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string FrontImageID { get; set; }
        public required string BackImageID { get; set; }
        public required string PersonalImage { get; set; }
        public UserVerificationStatus IdentityStatus { get; set; } = UserVerificationStatus.Pending;

        public required string CreditHoledrName { get; set; }
        public required string CreditNumber{ get; set;}
        public required string CreditExpirationDate { get; set; }

    }
}
