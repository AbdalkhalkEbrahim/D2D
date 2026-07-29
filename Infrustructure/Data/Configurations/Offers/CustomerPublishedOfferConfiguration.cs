using Domain.Entities.Offers;
using Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Offers
{
    public class CustomerPublishedOfferConfiguration : IEntityTypeConfiguration<CustomerPublishedOffer>
    {
        public void Configure(EntityTypeBuilder<CustomerPublishedOffer> builder)
        {

            builder.HasIndex(o => new { o.ID, o.IsActive })
                      .IsUnique()
                      .HasFilter("[IsActive] = 1");

            builder.HasMany(cpo => cpo.ProducerCustomerOffers)
                      .WithOne(pco => pco.CustomerPublishedOffer)
                      .HasForeignKey(pco => pco.CustomerPublishedOfferID)
                      .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany<ActiveOfferLogs>()
                      .WithOne()
                      .HasForeignKey(l => new { l.PublishedOfferID, l.IsPublishedOfferActive })
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
