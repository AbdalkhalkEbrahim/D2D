using Domain.Entities.Offers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations.Offers
{
    public class ProducerOfferConfiguration : IEntityTypeConfiguration<ProducerCustomerOffer>
    {
        public void Configure(EntityTypeBuilder<ProducerCustomerOffer> builder)
        {
            //builder.UseTpcMappingStrategy();

            builder.HasOne(po => po.Producer)
                    .WithMany(p => p.ProducerCustomerOffers)
                    .HasForeignKey(po => po.ProducerID)
                    .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
