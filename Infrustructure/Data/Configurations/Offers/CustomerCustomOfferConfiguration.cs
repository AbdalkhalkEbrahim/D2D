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
    public class CustomerCustomOfferConfiguration : IEntityTypeConfiguration<CustomerCustomOffer>
    {
        public void Configure(EntityTypeBuilder<CustomerCustomOffer> builder)
        {

            builder.HasIndex(o => new { o.ID, o.IsActive })
                      .IsUnique()
                      .HasFilter("[IsActive] = 1");

            builder.HasOne(cco => cco.ProducerDesign)
                      .WithMany()
                      .HasForeignKey(cco => cco.ProducerDesignID)
                      .OnDelete(DeleteBehavior.NoAction);

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
