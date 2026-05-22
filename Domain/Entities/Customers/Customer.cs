using Domain.Entities.Chats;
using Domain.Entities.Chats.AiModel;
using Domain.Entities.Designs;
using Domain.Entities.Offers;
using Domain.Entities.Producers;
using Domain.Entities.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual HashSet<Address> Addresses { get; set; }
        public virtual ICollection<CustomerOffer> Offers { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Chat> Chats { get; set; }
        public virtual ModelChat? ModelChat { get; set; }


        public Customer()
        {
            Addresses = new HashSet<Address>();
            Reviews = new List<Review>();
            Chats = new List<Chat>();
            Offers = new List<CustomerOffer>();

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
