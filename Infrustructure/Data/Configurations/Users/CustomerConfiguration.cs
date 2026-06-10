using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
