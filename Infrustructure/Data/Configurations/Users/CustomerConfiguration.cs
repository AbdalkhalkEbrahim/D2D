using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations.Users
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {

            builder.OwnsMany(c => c.Addresses, a =>
            {
                a.WithOwner(addr => addr.Customer)
                    .HasForeignKey(addr => addr.CustomerID);
                a.HasKey(addr => addr.ID);
            });
        }
    }
}
