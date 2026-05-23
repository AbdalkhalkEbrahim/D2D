using Domain.Entities.Offers;
using Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations.Offers
{
    public class CustomerCustomOfferConfiguration : IEntityTypeConfiguration<CustomerOffer>
    {
        public void Configure(EntityTypeBuilder<CustomerOffer> builder)
        {

            builder.HasIndex(o => new { o.ID, o.IsActive })
                      .IsUnique()
                      .HasFilter("[IsActive] = 1");

            builder.HasIndex(o => new { o.ID, o.Discrimerator })
                    .IsUnique()
                    .HasFilter("[Discrimerator] = 0"); //published offers

            builder.HasIndex(o => new { o.ID, o.Discrimerator })
                   .IsUnique()
                   .HasFilter("[Discrimerator] = 1"); //custom offers


            builder.HasMany<ActiveOfferLogs>()
                      .WithOne()
                      .HasForeignKey(l => new { l.OfferID, l.IsOfferActive })
                      .HasPrincipalKey(o => new { o.ID, o.IsActive })
                      .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany<Escrow>()
                      .WithOne()
                      .HasForeignKey(l => new { l.CustomerOfferID, l.IsOfferActive })
                      .HasPrincipalKey(o => new { o.ID, o.IsActive })
                      .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
