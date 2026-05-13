using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Domain.Entities.Shared;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Customers
{
    /// <summary>
    /// UserName
    /// Email
    /// PasswordHash
    /// </summary>
    public class Customer:User
    {
        public DateTime BD { get; set; }
        
        public virtual required HashSet<Address> Addresses { get; set; }
        public virtual ICollection<CustomerDesign> CustomerDesigns { get; set; }
        public ICollection<CustomerOffers> CustomerOffers { get; set; }

        public bool IsAllowed { get; }

        public Customer()
        {
            IsAllowed = DateTime.Now.Year - BD.Year >= 18;
            Addresses = new HashSet<Address>();
            CustomerDesigns = new List<CustomerDesign>();
            CustomerOffers = new List<CustomerOffers>();
        }
        public void SelectSpecificAddress(Address address)
        {
            foreach (var addr in Addresses)
            {
                if (addr.Equals(address)) 
                    addr.Selected = true;
                else
                    addr.Selected = false;
            }
        }

    }
}
